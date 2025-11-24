using H.Necessaire;
using H.Replication.Contracts;
using H.Replication.Contracts.DataContracts;

namespace H.Replication.MongoDB.Concrete
{
    internal class MongoDbReplicationRegistry : ImAnHReplicationRegistry
    {
        #region Construct
        readonly ImALogger log;
        readonly MongoDbInteractor mongoDbInteractor;
        public MongoDbReplicationRegistry(MongoDbInteractor mongoDbInteractor)
        {
            this.log = HApp.Deps.GetLogger<MongoDbReplicationRegistry>("H.Replication.MongoDB.Core");
            this.mongoDbInteractor = mongoDbInteractor;
        }
        #endregion

        public async Task<OperationResult<HReplicationRegistryEntry>> Append(HReplicationRequest replicationRequest)
        {
            if (!(await mongoDbInteractor.NewSession().LogError(log, "mongoDbInteractor.NewSession()")).Ref(out var sessRes, out var mongoSession))
                return sessRes.WithoutPayload<HReplicationRegistryEntry>();

            using (mongoSession)
            {
                return await HSafe.Run(async () =>
                {
                    HReplicationRegistryEntry replicationRegistryEntry = replicationRequest;
                    await mongoSession.ReplicationRegistryCollection.InsertOneAsync(replicationRegistryEntry);
                    return replicationRegistryEntry;
                });
            }
        }
    }
}
