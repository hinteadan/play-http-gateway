using H.Necessaire;
using H.Replication.Contracts;
using H.Replication.Contracts.DataContracts;

namespace H.Replication.Core
{
    internal class HReplicator : ImAnHReplicator
    {
        #region Construct
        readonly ImAnHReplicationRegistry replicationRegistry;
        readonly ImAnHReplicationProcessingQueue replicationProcessingQueue;
        readonly ImALogger log;
        public HReplicator(ImAnHReplicationRegistry replicationRegistry, ImAnHReplicationProcessingQueue replicationProcessingQueue)
        {
            this.replicationRegistry = replicationRegistry;
            this.replicationProcessingQueue = replicationProcessingQueue;
            this.log = HApp.Deps.GetLogger<HReplicator>("H.Replication.Core");
        }
        #endregion

        public async Task<OperationResult> Enqueue(HReplicationRequest replicationRequest)
        {
            if (!(await replicationRegistry.Append(replicationRequest).LogError(log, "replicationRegistry.Append(replicationRequest)")).Ref(out var regRes, out replicationRequest))
                return regRes;

            if (!(await replicationProcessingQueue.Enqueue(replicationRequest).LogError(log, "replicationProcessingQueue.Enqueue(replicationRequest)")).Ref(out var qRes, out replicationRequest))
                return qRes;

            return true;
        }
    }
}
