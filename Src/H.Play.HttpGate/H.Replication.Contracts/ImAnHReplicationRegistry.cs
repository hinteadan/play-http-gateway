using H.Necessaire;
using H.Replication.Contracts.DataContracts;
using System;
using System.Threading.Tasks;

namespace H.Replication.Contracts
{
    public interface ImAnHReplicationRegistry
    {
        Task<OperationResult<HReplicationRegistryEntry>> Append(HReplicationRequest replicationRequest);

        Task<OperationResult<HReplicationRegistryEntry>> Update(HReplicationRegistryEntry updatedReplicationRegistryEntry);

        Task<OperationResult<HReplicationRegistryEntry>> LoadEntry(Guid replicationRegistryEntryEntryID);

        Task<OperationResult<HReplicationRegistryEntry[]>> LoadReplicationHistoryFor(string payloadID, string payloadType);
    }
}
