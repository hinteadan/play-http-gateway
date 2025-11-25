using H.Necessaire;
using H.Replication.Contracts;
using H.Replication.Contracts.DataContracts;

namespace H.Replication.Core
{
    internal class HReplicator : ImAnHReplicator
    {
        #region Construct
        readonly ImAnHReplicationRegistry replicationRegistry;
        readonly ImAnHReplicationProcessingQueuePusher replicationProcessingQueue;
        readonly ImALogger log;
        public HReplicator(ImAnHReplicationRegistry replicationRegistry, ImAnHReplicationProcessingQueuePusher replicationProcessingQueue)
        {
            this.replicationRegistry = replicationRegistry;
            this.replicationProcessingQueue = replicationProcessingQueue;
            this.log = HApp.Deps.GetLogger<HReplicator>("H.Replication.Core");
        }
        #endregion

        public async Task<OperationResult> Enqueue(HReplicationRequest replicationRequest)
        {
            if (!(await replicationRegistry.Append(replicationRequest).LogError(log, "replicationRegistry.Append(replicationRequest)")).Ref(out var regRes, out var replicationRegistryEntry))
                return regRes;

            if (!(await replicationProcessingQueue.Enqueue(replicationRegistryEntry).LogError(log, "replicationProcessingQueue.Enqueue(replicationRegistryEntry)")).Ref(out var qRes, out var replicationQueueEntry))
                return qRes;

            return true;
        }
    }
}
