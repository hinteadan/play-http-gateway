using H.Necessaire;
using System;

namespace H.Replication.Contracts.DataContracts
{
    public class HReplicationRegistryEntry : EphemeralTypeBase, IGuidIdentity
    {
        public HReplicationRegistryEntry() => DoNotExpire();

        public Guid ID { get; set; } = Guid.NewGuid();

        public HReplicationStatus ReplicationStatus { get; set; }

        public HReplicationRequest ReplicationRequest { get; set; }

        public EphemeralType<OperationResult>[] ReplicationProcessingResults { get; set; }



        public static implicit operator HReplicationRegistryEntry(HReplicationRequest replicationRequest)
            => new HReplicationRegistryEntry { ReplicationRequest = replicationRequest };
    }
}
