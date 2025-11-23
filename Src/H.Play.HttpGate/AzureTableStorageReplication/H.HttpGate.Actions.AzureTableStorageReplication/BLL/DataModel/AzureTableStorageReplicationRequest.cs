using H.Necessaire;

namespace H.HttpGate.Actions.AzureTableStorageReplication.BLL.DataModel
{
    internal class AzureTableStorageReplicationRequest : EphemeralTypeBase, IGuidIdentity
    {
        public AzureTableStorageReplicationRequest() => ExpireIn(TimeSpan.FromDays(3));

        public Guid ID { get; set; } = Guid.NewGuid();

        public AzureTableStorageReplicationOperation ReplicationOperation { get; set; }

        public string AzureTableStorageTableName { get; set; }
        public string RowKey { get; set; }
        public string PartitionKey { get; set; }
        public string RawDataPayload { get; set; }
    }
}
