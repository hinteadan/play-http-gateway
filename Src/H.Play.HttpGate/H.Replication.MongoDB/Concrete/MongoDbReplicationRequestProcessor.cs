using H.Necessaire;
using H.Replication.Contracts;
using H.Replication.Contracts.DataContracts;
using H.Replication.MongoDB.AzureTableStorage;
using MongoDB.Bson;
using MongoDB.Driver;

namespace H.Replication.MongoDB.Concrete
{
    internal class MongoDbReplicationRequestProcessor : ImAnHReplicationRequestProcessor
    {
        #region Construct
        readonly ImALogger log;
        readonly MongoDbInteractor mongoDbInteractor;
        public MongoDbReplicationRequestProcessor(MongoDbInteractor mongoDbInteractor)
        {
            this.log = HApp.Deps.GetLogger<MongoDbReplicationRegistry>("H.Replication.MongoDB");
            this.mongoDbInteractor = mongoDbInteractor;
        }
        #endregion

        public async Task<OperationResult> Process(HReplicationRequest replicationRequest)
        {
            if (!(await replicationRequest.ToMongoReplicationOperation().LogError(log, "replicationRequest.ToMongoReplicationOperation()")).Ref(out var parseRes, out var replicationOperation))
                return parseRes;

            if (replicationOperation is null)
                return "replicationOperation is null";

            if (!(await mongoDbInteractor.NewSession().LogError(log, "mongoDbInteractor.NewSession()")).Ref(out var sessRes, out var mongoSession))
                return sessRes.WithoutPayload<HReplicationRegistryEntry>();

            if ((replicationOperation.AzureTableStorageTableName).IsEmpty())
                return "AzureTableStorageTableName is empty";
            if ((replicationOperation.PartitionKey).IsEmpty())
                return "PartitionKey is empty";
            if ((replicationOperation.RowKey).IsEmpty())
                return "RowKey is empty";

            using (mongoSession)
            {
                return
                    await HSafe.Run(async () =>
                    {
                        string collectionName = replicationOperation.AzureTableStorageTableName;
                        IMongoCollection<BsonDocument> mongoCollection = mongoSession.ReplicationRegistryDatabase.GetCollection<BsonDocument>(collectionName);

                        switch (replicationOperation.ReplicationOperation)
                        {
                            case HReplicationOperation.Delete:
                                await mongoCollection.DeleteOneAsync(x => x["PartitionKey"] == replicationOperation.PartitionKey && x["RowKey"] == replicationOperation.RowKey);
                                break;
                            case HReplicationOperation.Upsert:
                            case HReplicationOperation.Insert:
                            case HReplicationOperation.Update:
                            case HReplicationOperation.Merge:
                            default:
                                if (replicationOperation.Document is null)
                                    throw new OperationResultException("replicationOperation.Document is null");
                                BsonDocument newDoc = replicationOperation.Document;
                                await mongoCollection.ReplaceOneAsync(x => x["PartitionKey"] == replicationOperation.PartitionKey && x["RowKey"] == replicationOperation.RowKey, newDoc, new ReplaceOptions { IsUpsert = true });
                                break;
                        }

                    });
            }
        }
    }
}
