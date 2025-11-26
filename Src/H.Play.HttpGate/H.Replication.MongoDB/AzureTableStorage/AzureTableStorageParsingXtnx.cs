using H.Necessaire;
using H.Replication.Contracts.DataContracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Text.RegularExpressions;

namespace H.Replication.MongoDB.AzureTableStorage
{
    internal static class AzureTableStorageParsingXtnx
    {
        const string hRequestPayloadIDConcatenator = "/";
        /**
         * 
         * Insert Entity
         * =============
         * https://learn.microsoft.com/en-us/rest/api/storageservices/insert-entity
         * URI Params: No need
         * Req Headers: No need
         * Req Body: JSON with the entity itself
         * {  
               "Address":"Mountain View",  
               "Age":23,  
               "AmountDue":200.23,  
               "CustomerCode@odata.type":"Edm.Guid",  
               "CustomerCode":"c9da6455-213d-42c9-9a79-3e9149a57833",  
               "CustomerSince@odata.type":"Edm.DateTime",  
               "CustomerSince":"2008-07-10T00:00:00",  
               "IsActive":true,  
               "NumberOfOrders@odata.type":"Edm.Int64",  
               "NumberOfOrders":"255",  
               "PartitionKey":"mypartitionkey",  
               "RowKey":"myrowkey"  
            }
         * 
         * 
         * Insert Or Merge Entity
         * ======================
         * https://learn.microsoft.com/en-us/rest/api/storageservices/insert-or-merge-entity
         * URI Params: No need
         * Req Headers: No need
         * Req Body: JSON with the entity itself, like insert
         * 
         * 
         * Insert Or Replace Entity
         * ========================
         * Same
         * 
         * 
         * Update Entity
         * =============
         * Same
         * 
         * 
         * Merge Entity
         * ============
         * Same
         * 
         * 
         * Delete Entity
         * =============
         * Row key and partition key are already parsed from the URI and are stored in the payload props
         * Req Body: none
         * 
         */

        public static OperationResult<AzureTableStorageToMongoReplicationOperation> ToMongoReplicationOperation(this HReplicationRequest replicationRequest)
        {
            if (replicationRequest is null)
                return "replicationRequest is null";

            if (replicationRequest.PayloadType != "AzureTableStorage")
                return "Replication Request Payload Type is not AzureTableStorage";

            if ((replicationRequest.PayloadContentType?.IndexOf("application/json", StringComparison.InvariantCultureIgnoreCase) ?? -1) < 0)
                return "Replication Request PayloadContentType Type is unknown";

            string[] idParts = replicationRequest.PayloadID?.Split(hRequestPayloadIDConcatenator, count: 3, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (idParts?.Length != 3)
                return "replicationRequest.PayloadID is invalid as it cannot be parsed to AzureTableStorageTableName/PartitionKey/RowKey";

            return
                new AzureTableStorageToMongoReplicationOperation
                {
                    AzureTableStorageTableName = replicationRequest.PayloadGroup.IfEmpty(idParts[0]),
                    PartitionKey = idParts[1],
                    RowKey = idParts[2],
                    ReplicationOperation = replicationRequest.Operation,
                    Document = replicationRequest.Payload.MorphIfNotEmpty(x => BsonSerializer.Deserialize<BsonDocument>(x.RemoveODataMetaProps()), defaultTo: null),
                }
                ;
        }

        static readonly Regex oDataMetaPropsRegex = new Regex(",\"[^\"]+@odata\\.type\":\"[^\"]+\"");
        static readonly Regex numericPropValuePropsRegex = new Regex(":\"(\\d+)\"");
        static string RemoveODataMetaProps(this string json)
        {
            if (json.IsEmpty())
                return json;
            string result = json;
            result = oDataMetaPropsRegex.Replace(result, string.Empty);
            result = numericPropValuePropsRegex.Replace(result, ":$1");
            return result;
        }
    }
}
