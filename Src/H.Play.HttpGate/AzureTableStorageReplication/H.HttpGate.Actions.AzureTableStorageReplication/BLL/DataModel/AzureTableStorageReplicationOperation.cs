namespace H.HttpGate.Actions.AzureTableStorageReplication.BLL.DataModel
{
    internal enum AzureTableStorageReplicationOperation : sbyte
    {
        Delete = -10,

        Upsert = 0,

        Insert = 1,

        Update = 10,

        Merge = 11,
    }
}
