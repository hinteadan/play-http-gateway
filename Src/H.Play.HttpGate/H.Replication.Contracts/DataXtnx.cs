using H.Necessaire;
using H.Replication.Contracts.DataContracts;

namespace H.Replication.Contracts
{
    public static class DataXtnx
    {
        static readonly object appendProcessingResultLocker = new object();

        public static HReplicationRegistryEntry AppendProcessingResult(this HReplicationRegistryEntry replicationRegistryEntry, EphemeralType<OperationResult> result)
        {
            if (replicationRegistryEntry is null)
                return null;

            lock (appendProcessingResultLocker)
            {
                replicationRegistryEntry.ReplicationProcessingResults = replicationRegistryEntry.ReplicationProcessingResults.Push(result, checkDistinct: false);

                return replicationRegistryEntry;
            }
        }
    }
}
