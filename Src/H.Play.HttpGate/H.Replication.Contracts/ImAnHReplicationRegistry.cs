using H.Necessaire;
using H.Replication.Contracts.DataContracts;
using System.Threading.Tasks;

namespace H.Replication.Contracts
{
    public interface ImAnHReplicationRegistry
    {
        Task<OperationResult<HReplicationRequest>> Append(HReplicationRequest replicationRequest);
    }
}
