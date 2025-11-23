namespace H.Replication.Contracts.DataContracts
{
    public enum HReplicationOperation : sbyte
    {
        Delete = -10,

        Upsert = 0,

        Insert = 1,

        Update = 10,

        Merge = 11,
    }
}
