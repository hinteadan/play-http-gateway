using H.HttpGate.Actions.AzureTableStorageReplication.BLL.DataModel;
using H.HttpGate.Contracts.Public.DataContracts;
using H.Necessaire;
using H.Necessaire.Serialization;
using H.Replication.Contracts.DataContracts;

namespace H.HttpGate.Actions.AzureTableStorageReplication.BLL
{
    internal static class MappingXtnx
    {
        const string hRequestPayloadIDConcatenator = "/";
        public static OperationResult<AzureTableStorageReplicationRequest> ToReplicationRequest(this HHttpGateResponse hHttpGateResponse)
        {
            if (!IsEligible(hHttpGateResponse).Ref(out var eligibleRes))
                return eligibleRes.WithoutPayload<AzureTableStorageReplicationRequest>();

            Uri uri = new Uri(hHttpGateResponse.Request.URL);
            string[] relevantUriParts = uri.LocalPath?.Split(["/", "(", ")", ","], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            string azureTableName = relevantUriParts?.FirstOrDefault();
            if (azureTableName.IsEmpty())
                return $"Cannot determine table name from URL: {hHttpGateResponse.Request.URL}";

            string partitionKeyFromUrl = ParseTsUrlQueryParam(relevantUriParts, "PartitionKey");
            string rowKeyFromUrl = ParseTsUrlQueryParam(relevantUriParts, "RowKey");

            if (!DetermineReplicationOperation(hHttpGateResponse).Ref(out var repOpRes, out var replicationOperation))
                return repOpRes.WithoutPayload<AzureTableStorageReplicationRequest>();

            TSEntity tsEntity = hHttpGateResponse.Request.Content.JsonToObject<TSEntity>();

            return
                new AzureTableStorageReplicationRequest
                {
                    AzureTableStorageTableName = azureTableName,
                    ReplicationOperation = replicationOperation,
                    RowKey = (tsEntity?.RowKey).IfEmpty(rowKeyFromUrl),
                    PartitionKey = (tsEntity?.PartitionKey).IfEmpty(partitionKeyFromUrl),
                    RawDataPayload = hHttpGateResponse.Request.Content
                }
                ;
        }

        public static OperationResult<HReplicationRequest> ToHReplicationRequest(this AzureTableStorageReplicationRequest azureTableStorageReplicationRequest)
        {
            if (azureTableStorageReplicationRequest is null)
                return "azureTableStorageReplicationRequest is null";

            return HSafe.Run(() =>
            {

                return new HReplicationRequest
                {
                    ID = azureTableStorageReplicationRequest.ID,
                    Source = nameof(AzureTableStorageReplicationRequest),
                    Operation = (HReplicationOperation)(sbyte)azureTableStorageReplicationRequest.ReplicationOperation,

                    PayloadContentType = "application/json",
                    PayloadType = "AzureTableStorage",
                    PayloadGroup = azureTableStorageReplicationRequest.AzureTableStorageTableName,
                    PayloadID = string.Join(hRequestPayloadIDConcatenator, new string[] { azureTableStorageReplicationRequest.AzureTableStorageTableName, azureTableStorageReplicationRequest.PartitionKey, azureTableStorageReplicationRequest.RowKey }.ToNonEmptyArray()),
                    Payload = azureTableStorageReplicationRequest.RawDataPayload,

                    AsOf = azureTableStorageReplicationRequest.AsOf,
                    CreatedAt = azureTableStorageReplicationRequest.CreatedAt,
                    ValidFrom = azureTableStorageReplicationRequest.ValidFrom,
                    ExpiresAt = azureTableStorageReplicationRequest.ExpiresAt,
                    ValidFor = azureTableStorageReplicationRequest.ValidFor,
                }
                ;

            });
        }

        static OperationResult IsEligible(HHttpGateResponse hHttpGateResponse)
        {
            if (hHttpGateResponse is null)
                return "hHttpGateResponse is null";

            if (hHttpGateResponse.Request is null)
                return "hHttpGateResponse.Request is null";

            if (hHttpGateResponse.Request.URL.IsEmpty())
                return "Request URL is empty";

            if ((hHttpGateResponse?.Request?.HttpMethod).IsEmpty())
                return "HTTP Method is empty, cannot determine relication operation";

            if (hHttpGateResponse.Request.HttpMethod.Is("GET") || hHttpGateResponse.Request.HttpMethod.Is("HEAD"))
                return OperationResult.Fail("Read operation doesn't need replication").WithComment("DoNotLog");

            if (hHttpGateResponse.Request.HttpMethod.Is("OPTIONS"))
                return OperationResult.Fail("Preflight operation doesn't need replication").WithComment("DoNotLog");

            if (hHttpGateResponse.Request.URL.IndexOf("table.core.windows.net/", StringComparison.InvariantCultureIgnoreCase) < 0)
                return OperationResult.Fail("Request URL is not for Azure Table Storage").WithComment("DoNotLog");

            if (hHttpGateResponse.Request.URL.IndexOf("/Tables", StringComparison.InvariantCultureIgnoreCase) >= 0
                || hHttpGateResponse.Request.URL.IndexOf("/?restype=service&comp=properties", StringComparison.InvariantCultureIgnoreCase) >= 0
                || (hHttpGateResponse.Request.HttpMethod.Is("PUT") && hHttpGateResponse.Request.URL.IndexOf("?comp=acl", StringComparison.InvariantCultureIgnoreCase) >= 0)
                )
                return OperationResult.Fail("Azure Table Storage Table Operations are not replicated").WithComment("DoNotLog");

            if (hHttpGateResponse.HttpStatusCode < 200 || hHttpGateResponse.HttpStatusCode >= 300)
                return $"Response Status Code {hHttpGateResponse.HttpStatusCode} does not indicate a successful operationm thus we'll not replicate";

            return true;
        }

        static OperationResult<AzureTableStorageReplicationOperation> DetermineReplicationOperation(HHttpGateResponse hHttpGateResponse)
        {
            if (hHttpGateResponse.Request.HttpMethod.Is("POST"))
                return AzureTableStorageReplicationOperation.Insert;

            if (hHttpGateResponse.Request.HttpMethod.Is("PUT"))
                return AzureTableStorageReplicationOperation.Upsert;

            if (hHttpGateResponse.Request.HttpMethod.Is("MERGE"))
                return AzureTableStorageReplicationOperation.Merge;

            if (hHttpGateResponse.Request.HttpMethod.Is("DELETE"))
                return AzureTableStorageReplicationOperation.Delete;

            return $"HTTP Method {hHttpGateResponse.Request.HttpMethod} is not supported for replication";
        }

        static string ParseTsUrlQueryParam(string[] parts, string paramName)
        {
            if (parts.IsEmpty())
                return null;

            string param = parts.FirstOrDefault(x => x.IndexOf(paramName, StringComparison.InvariantCultureIgnoreCase) >= 0);
            if (param.IsEmpty())
                return null;

            string[] paramParts = param.Split("=", 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (paramParts.Length < 2)
                return null;

            string value = paramParts[1];

            if (value.StartsWith('\'') || value.StartsWith('"'))
                value = value.Substring(1);

            if (value.EndsWith('\'') || value.EndsWith('"'))
                value = value.Substring(0, value.Length - 1);

            return value;
        }


        class TSEntity
        {
            public string RowKey { get; set; }
            public string PartitionKey { get; set; }
        }
    }
}
