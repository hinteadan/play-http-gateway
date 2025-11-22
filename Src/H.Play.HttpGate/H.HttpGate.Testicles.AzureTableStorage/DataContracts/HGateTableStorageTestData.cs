using Azure;
using Azure.Data.Tables;
using H.Necessaire;

namespace H.HttpGate.Testicles.AzureTableStorage.DataContracts
{
    public class HGateTableStorageTestData : EphemeralTypeBase, ITableEntity, IStringIdentity
    {
        const string idSeparator = "::";
        public string ID => $"{PartitionKey}{idSeparator}{RowKey}";

        public string DisplayName { get; set; }
        public string Description { get; set; }


        public string PartitionKey { get; set; } = typeof(HGateTableStorageTestData).Name;
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
