using H.Necessaire;
using H.Replication.Contracts.DataContracts;
using System.Threading.Tasks;

namespace H.Replication.Contracts
{
    public interface ImAnHReplicationProcessingQueuePusher
    {
        Task<OperationResult<HReplicationProcessingQueueEntry>> Enqueue(HReplicationRegistryEntry replicationRegistryEntry);
    }
}
