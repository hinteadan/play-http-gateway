using H.Necessaire;
using H.Replication.Contracts;
using H.Replication.Contracts.DataContracts;

namespace H.Replication.DataCopy.Processor
{
    internal class HReplicationProcessingQueueProcessor : ImAnHReplicationProcessingQueueProcessor
    {
        #region Construct
        readonly ImALogger log;
        readonly ImAnHReplicationRegistry replicationRegistry;
        readonly ImAnHReplicationRequestProcessor replicationRequestProcessor;
        public HReplicationProcessingQueueProcessor(ImAnHReplicationRegistry replicationRegistry, ImAnHReplicationRequestProcessor replicationRequestProcessor)
        {
            this.replicationRegistry = replicationRegistry;
            this.replicationRequestProcessor = replicationRequestProcessor;
            log = HApp.Deps.GetLogger<HReplicationProcessingQueueProcessor>("H.Replication.DataCopy.Processor");
        }
        #endregion

        public async Task<OperationResult> Process(HReplicationProcessingQueueEntry replicationProcessingQueueEntry)
        {
            OperationResult result = await HSafe.RunWithRetry(
                async () => await RunProcessingAttempt(replicationProcessingQueueEntry),
                res => res
            ).LogError(log, "RunProcessingAttempt with retry");

            return result;
        }

        async Task<OperationResult> RunProcessingAttempt(HReplicationProcessingQueueEntry replicationProcessingQueueEntry)
        {
            if (!(await replicationRegistry.LoadEntry(replicationProcessingQueueEntry.ReplicationRegistryEntryID).LogError(log, $"replicationRegistry.LoadEntry({replicationProcessingQueueEntry.ReplicationRegistryEntryID})")).Ref(out var loadRes, out var replicationRegistryEntry))
                return loadRes;

            #region Prechecks
            if (replicationRegistryEntry.ReplicationStatus != HReplicationStatus.Pending)
                return
                    replicationRegistryEntry.ReplicationStatus == HReplicationStatus.Processing
                    ? OperationResult.Win().Warn($"Replication with registry ID {replicationRegistryEntry.ID} is already being processed")
                    : OperationResult.Win().Warn($"Replication with registry ID {replicationRegistryEntry.ID} is already processed")
                    ;

            if (replicationRegistryEntry.IsExpired())
            {
                replicationRegistryEntry.ReplicationStatus = HReplicationStatus.Expired;
                replicationRegistryEntry.AppendProcessingResult(OperationResult.Win().Warn($"Replication with registry ID {replicationRegistryEntry.ID} has expired, will be ignored").Ref(out var expiredRes));

                if (!(await replicationRegistry.Update(replicationRegistryEntry).LogError(log, "replicationRegistry.Update")).Ref(out var updateRes, out replicationRegistryEntry))
                    return updateRes;

                return expiredRes;
            }

            replicationRegistryEntry.ReplicationStatus = HReplicationStatus.Processing;
            if (!(await replicationRegistry.Update(replicationRegistryEntry).LogError(log, "replicationRegistry.Update")).Ref(out var upRes, out replicationRegistryEntry)) return upRes;

            if (!(await replicationRegistry.LoadReplicationHistoryFor(replicationRegistryEntry.ReplicationRequest.PayloadID, replicationRegistryEntry.ReplicationRequest.PayloadType).LogError(log, $"replicationRegistry.LoadReplicationHistoryFor({replicationRegistryEntry.ReplicationRequest.PayloadID}, {replicationRegistryEntry.ReplicationRequest.PayloadType})")).Ref(out var loadHisRes, out var historyEntries))
            {
                replicationRegistryEntry.ReplicationStatus = HReplicationStatus.Failed;
                replicationRegistryEntry.AppendProcessingResult(loadHisRes);

                if (!(await replicationRegistry.Update(replicationRegistryEntry).LogError(log, "replicationRegistry.Update")).Ref(out upRes, out replicationRegistryEntry))
                    return new OperationResult[] { loadHisRes, upRes }.Merge();

                return loadHisRes;
            }

            HReplicationRegistryEntry newerReplication = historyEntries?.FirstOrDefault(x => x.ReplicationRequest.ValidFrom >= replicationRegistryEntry.ReplicationRequest.ValidFrom);
            if (newerReplication != null)
            {
                replicationRegistryEntry.ReplicationStatus = HReplicationStatus.AlreadyReplicated;
                replicationRegistryEntry.AppendProcessingResult(OperationResult.Win($"Replication with registry ID {replicationRegistryEntry.ID} is already replicated by a newer request with registry id {newerReplication.ID}").Ref(out var winRes));

                if (!(await replicationRegistry.Update(replicationRegistryEntry).LogError(log, "replicationRegistry.Update")).Ref(out var updateRes, out replicationRegistryEntry))
                    return updateRes;

                return winRes;
            }
            #endregion PreChecks


            #region Actual Data Replication since we passed all the prechecks
            if (!(await ProcessReplicationRequest(replicationRegistryEntry.ReplicationRequest).LogError(log, "ProcessReplicationRequest")).RefTo(out var repReqRes))
            {
                replicationRegistryEntry.ReplicationStatus = HReplicationStatus.Failed;
                replicationRegistryEntry.AppendProcessingResult(repReqRes);

                if (!(await replicationRegistry.Update(replicationRegistryEntry).LogError(log, "replicationRegistry.Update")).Ref(out upRes, out replicationRegistryEntry))
                    return new OperationResult[] { repReqRes, upRes }.Merge();

                return repReqRes;
            }

            replicationRegistryEntry.ReplicationStatus = HReplicationStatus.Succeeded;
            replicationRegistryEntry.AppendProcessingResult(OperationResult.Win());

            if (!(await replicationRegistry.Update(replicationRegistryEntry).LogError(log, "replicationRegistry.Update")).Ref(out upRes, out replicationRegistryEntry))
                return upRes;
            #endregion Actual Data Replication since we passed all the prechecks


            return OperationResult.Win();
        }

        async Task<OperationResult> ProcessReplicationRequest(HReplicationRequest replicationRequest)
        {
            #region Prechecks
            if (replicationRequest is null)
                return "Replication Request is null";

            if (replicationRequest.IsExpired())
                return "Replication Request is expired";
            #endregion

            return await replicationRequestProcessor.Process(replicationRequest).LogError(log, "replicationRequestProcessor.Process(replicationRequest)");
        }
    }
}
