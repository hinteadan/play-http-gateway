using H.Config;
using H.Necessaire;
using Microsoft.Extensions.Configuration;

namespace H.Replication.AzureServiceBus
{
    internal class AzureServiceBusConfigProvider
    {
        #region Construct
        readonly Lazy<AzureServiceBusConfig> lazyConfig;
        readonly IConfiguration configuration;
        public AzureServiceBusConfigProvider(IConfiguration configuration = null)
        {
            this.configuration = configuration;
            lazyConfig = new Lazy<AzureServiceBusConfig>(LoadConfig);
        }
        #endregion

        public OperationResult<AzureServiceBusConfig> GetConfig()
        {
            return HSafe.Run(() => lazyConfig.Value);
        }

        AzureServiceBusConfig LoadConfig()
        {
            return
                configuration.GetConfigSection("HReplication", "AzureServiceBus").MorphIfNotNull(x => new AzureServiceBusConfig
                {
                    Keys = x.GetConfigValues(nameof(AzureServiceBusConfig.Keys)),
                    ConnectionStrings = x.GetConfigValues(nameof(AzureServiceBusConfig.ConnectionStrings)),
                    ReplicationQueueName = x.GetConfigValue(nameof(AzureServiceBusConfig.ReplicationQueueName)),
                })
                ??
                LoadConfigFromEmbeddedSecrets()
                ;
        }

        static AzureServiceBusConfig LoadConfigFromEmbeddedSecrets()
        {
            return
                new AzureServiceBusConfig
                {
                    Keys = "AzureSBKeys.cfg.txt".ReadSecretFromEmbeddedResources(typeof(AzureServiceBusConfigProvider).Assembly)?.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)?.ToNonEmptyArray(),
                    ConnectionStrings = "AzureSBConnectionStrings.cfg.txt".ReadSecretFromEmbeddedResources(typeof(AzureServiceBusConfigProvider).Assembly)?.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)?.ToNonEmptyArray(),
                    ReplicationQueueName = "AzureSBReplicationProcessingQueueName.cfg.txt".ReadSecretFromEmbeddedResources(typeof(AzureServiceBusConfigProvider).Assembly)?.Trim(),
                }
                ;
        }
    }

    internal class AzureServiceBusConfig
    {
        public string[] Keys { get; set; }
        public string[] ConnectionStrings { get; set; }
        public string ReplicationQueueName { get; set; }
    }
}
