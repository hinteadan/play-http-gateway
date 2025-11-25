namespace H.Replication.Contracts.DataContracts
{
    public enum HReplicationStatus : sbyte
    {
        Failed = -100,

        Pending = 0,

        Processing = 1,

        Succeeded = 100,

        AlreadyReplicated = 103,

        Expired = 127,
    }
}
