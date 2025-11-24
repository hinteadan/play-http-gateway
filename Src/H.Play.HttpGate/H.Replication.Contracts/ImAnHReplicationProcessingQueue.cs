using H.Necessaire;
using H.Replication.Contracts.DataContracts;
using System.Threading.Tasks;

namespace H.Replication.Contracts
{
    public interface ImAnHReplicationProcessingQueue
    {
        Task<OperationResult<HReplicationRegistryEntry>> Enqueue(HReplicationRegistryEntry replicationRegistryEntry);
    }
}
