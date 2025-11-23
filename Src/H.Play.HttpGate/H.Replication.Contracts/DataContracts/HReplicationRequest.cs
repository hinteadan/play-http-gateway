using H.Necessaire;
using System;

namespace H.Replication.Contracts.DataContracts
{
    public class HReplicationRequest : EphemeralTypeBase, IGuidIdentity
    {
        static readonly TimeSpan defaultValidity = TimeSpan.FromDays(3);
        public HReplicationRequest() => ExpireIn(defaultValidity);

        public Guid ID { get; set; } = Guid.NewGuid();
        public HReplicationOperation Operation { get; set; }
        public string Source { get; set; }

        public string PayloadID { get; set; }
        public string PayloadType { get; set; }
        public string PayloadContentType { get; set; }
        public string Payload { get; set; }
    }
}
