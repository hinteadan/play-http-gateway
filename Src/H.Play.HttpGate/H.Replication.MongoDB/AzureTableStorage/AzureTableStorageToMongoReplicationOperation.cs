using H.Replication.Contracts.DataContracts;
using MongoDB.Bson;

namespace H.Replication.MongoDB.AzureTableStorage
{
    internal class AzureTableStorageToMongoReplicationOperation
    {
        public HReplicationOperation ReplicationOperation { get; set; }
        public string AzureTableStorageTableName { get; set; }
        public string RowKey { get; set; }
        public string PartitionKey { get; set; }
        public BsonDocument Document { get; set; }
    }
}
