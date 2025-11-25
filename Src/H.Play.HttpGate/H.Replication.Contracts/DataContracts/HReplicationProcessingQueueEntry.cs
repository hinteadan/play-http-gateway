using H.Necessaire;
using System;

namespace H.Replication.Contracts.DataContracts
{
    public class HReplicationProcessingQueueEntry : EphemeralTypeBase, IGuidIdentity
    {
        public HReplicationProcessingQueueEntry() => ExpireIn(TimeSpan.FromDays(3));

        public Guid ID { get; set; } = Guid.NewGuid();

        public Guid ReplicationRegistryEntryID { get; set; }

        public Guid ReplicationRequestID { get; set; }



        public static implicit operator HReplicationProcessingQueueEntry(HReplicationRegistryEntry replicationRegistryEntry)
            => new HReplicationProcessingQueueEntry
            {
                ReplicationRegistryEntryID = replicationRegistryEntry?.ID ?? Guid.Empty,
                ReplicationRequestID = replicationRegistryEntry?.ReplicationRequest?.ID ?? Guid.Empty,
            };
    }
}
