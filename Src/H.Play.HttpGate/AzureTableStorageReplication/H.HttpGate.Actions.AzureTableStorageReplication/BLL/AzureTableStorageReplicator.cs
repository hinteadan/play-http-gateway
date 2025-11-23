using H.HttpGate.Actions.AzureTableStorageReplication.BLL.DataModel;
using H.Necessaire;
using H.Replication.Contracts;
using System.Collections.Concurrent;

namespace H.HttpGate.Actions.AzureTableStorageReplication.BLL
{
    internal class AzureTableStorageReplicator
    {
        #region Construct
        static readonly SemaphoreSlim runReplicationSessionSemaphore = new SemaphoreSlim(1, 1);
        static readonly ConcurrentQueue<AzureTableStorageReplicationRequest> replicationQueue = new();
        readonly ImAnHReplicator replicator;
        readonly ImALogger log;
        public AzureTableStorageReplicator(ImAnHReplicator replicator)
        {
            this.replicator = replicator;
            this.log = HApp.Deps.GetLogger<AzureTableStorageReplicator>("HttpGate.Actions.AzureTableStorageReplication");
        }
        #endregion

        public async Task<OperationResult> QueueReplication(AzureTableStorageReplicationRequest replicationRequest)
        {
            replicationQueue.Enqueue(replicationRequest);

            return true;
        }

        public async Task ShutdownASAP()
        {
            if (replicationQueue.IsEmpty)
                return;

            await RunReplicationSession(CancellationToken.None);
        }

        public async Task RunReplicationSession(CancellationToken cancelToken)
        {
            await runReplicationSessionSemaphore.WaitAsync();

            try
            {
                if (replicationQueue.IsEmpty)
                    return;

                while (replicationQueue.TryDequeue(out AzureTableStorageReplicationRequest queueEntry))
                {
                    await HSafe.Run(async () =>
                    {
                        if (!(await queueEntry.ToHReplicationRequest().LogError(log, "queueEntry.ToHReplicationRequest")).RefPayload(out var hReplicationRequest))
                            return;

                        if (!(await replicator.Enqueue(hReplicationRequest).LogError(log, "replicator.Enqueue(hReplicationRequest)")))
                            return;
                    });
                }
            }
            finally
            {
                runReplicationSessionSemaphore.Release();
            }
        }
    }
}
