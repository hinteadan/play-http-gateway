using H.Config;
using H.Necessaire;
using Microsoft.Extensions.Configuration;

namespace H.Replication.MongoDB
{
    internal class MongoDbConfigProvider
    {
        #region Construct
        readonly Lazy<MongoDbConfig> lazyConfig;
        readonly IConfiguration configuration;
        public MongoDbConfigProvider(IConfiguration configuration = null)
        {
            this.configuration = configuration;
            lazyConfig = new Lazy<MongoDbConfig>(LoadConfig);
        }
        #endregion


        public OperationResult<MongoDbConfig> GetMongoDbConfig()
        {
            return HSafe.Run(() => lazyConfig.Value);
        }

        MongoDbConfig LoadConfig()
        {
            return
                configuration.GetConfigSection("HReplication", "MongoDB").MorphIfNotNull(x => new MongoDbConfig
                {
                    ConnectionString = x.GetConfigValue(nameof(MongoDbConfig.ConnectionString)),
                    ReplicationRegistryDatabaseName = x.GetConfigValue(nameof(MongoDbConfig.ReplicationRegistryDatabaseName)),
                    ReplicationRegistryCollectionName = x.GetConfigValue(nameof(MongoDbConfig.ReplicationRegistryCollectionName)),
                })
                ??
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
