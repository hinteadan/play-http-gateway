using H.Necessaire;
using H.Replication.Contracts;
using H.Replication.Contracts.DataContracts;
using MongoDB.Driver;

namespace H.Replication.MongoDB.Concrete
{
    internal class MongoDbReplicationRegistry : ImAnHReplicationRegistry
    {
        #region Construct
        readonly ImALogger log;
        readonly MongoDbInteractor mongoDbInteractor;
        public MongoDbReplicationRegistry(MongoDbInteractor mongoDbInteractor)
        {
            this.log = HApp.Deps.GetLogger<MongoDbReplicationRegistry>("H.Replication.MongoDB");
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

        public async Task<OperationResult<HReplicationRegistryEntry>> Update(HReplicationRegistryEntry updatedReplicationRegistryEntry)
        {
            if (!(await mongoDbInteractor.NewSession().LogError(log, "mongoDbInteractor.NewSession()")).Ref(out var sessRes, out var mongoSession))
                return sessRes.WithoutPayload<HReplicationRegistryEntry>();

            using (mongoSession)
            {
                return await HSafe.Run(async () =>
                {
                    updatedReplicationRegistryEntry.AsOf = DateTime.UtcNow;
                    await mongoSession.ReplicationRegistryCollection.ReplaceOneAsync(x => x.ID == updatedReplicationRegistryEntry.ID, updatedReplicationRegistryEntry);
                    return updatedReplicationRegistryEntry;
                });
            }
        }

        public async Task<OperationResult<HReplicationRegistryEntry>> LoadEntry(Guid replicationRegistryEntryEntryID)
        {
            if (!(await mongoDbInteractor.NewSession().LogError(log, "mongoDbInteractor.NewSession()")).Ref(out var sessRes, out var mongoSession))
                return sessRes.WithoutPayload<HReplicationRegistryEntry>();

            using (mongoSession)
            {
                return await HSafe.Run(async () =>
                {
                    HReplicationRegistryEntry entry = await (await mongoSession.ReplicationRegistryCollection.FindAsync(x => x.ID == replicationRegistryEntryEntryID)).SingleOrDefaultAsync();

                    if (entry is null)
                        throw new OperationResultException($"ReplicationRegistryEntry {replicationRegistryEntryEntryID} doesn't exist");

                    return entry;
                });
            }
        }

        public async Task<OperationResult<HReplicationRegistryEntry[]>> LoadReplicationHistoryFor(string payloadID, string payloadType)
        {
            if (!(await mongoDbInteractor.NewSession().LogError(log, "mongoDbInteractor.NewSession()")).Ref(out var sessRes, out var mongoSession))
                return sessRes.WithoutPayload<HReplicationRegistryEntry[]>();

            using (mongoSession)
            {
                return await HSafe.Run(async () =>
                {
                    List<HReplicationRegistryEntry> results = await (
                        await mongoSession.ReplicationRegistryCollection.FindAsync(
                            x => x.ReplicationRequest.PayloadID == payloadID
                            && x.ReplicationRequest.PayloadType == payloadType
                            && x.ReplicationStatus >= HReplicationStatus.Succeeded
                        )
                    ).ToListAsync()
                    ;

                    return results?.ToNoNullsArray();
                });
            }
        }
    }
}
