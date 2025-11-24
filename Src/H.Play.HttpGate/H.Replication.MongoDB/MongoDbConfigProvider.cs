using H.Necessaire;

namespace H.Replication.MongoDB
{
    internal class MongoDbConfigProvider
    {
        static readonly Lazy<MongoDbConfig> lazyMongoDbConfig = new Lazy<MongoDbConfig>(LoadMongoDbConfigFromEmbeddedSecrets);

        public OperationResult<MongoDbConfig> GetMongoDbConfig()
        {
            return HSafe.Run(() => lazyMongoDbConfig.Value);
        }

        static MongoDbConfig LoadMongoDbConfigFromEmbeddedSecrets()
        {
            return
                new MongoDbConfig
                {
                    ConnectionString = "MongoDbConnectionString.cfg.txt".ReadSecretFromEmbeddedResources(typeof(MongoDbConfigProvider).Assembly)?.Trim(),
                    ReplicationRegistryDatabaseName = "MongoDbReplicationRegistryDatabaseName.cfg.txt".ReadSecretFromEmbeddedResources(typeof(MongoDbConfigProvider).Assembly)?.Trim(),
                    ReplicationRegistryCollectionName = "MongoDbReplicationRegistryCollectionName.cfg.txt".ReadSecretFromEmbeddedResources(typeof(MongoDbConfigProvider).Assembly)?.Trim(),
                }
                ;
        }
    }

    internal class MongoDbConfig
    {
        public string ConnectionString { get; set; }
        public string ReplicationRegistryDatabaseName { get; set; }
        public string ReplicationRegistryCollectionName { get; set; }
    }
}
