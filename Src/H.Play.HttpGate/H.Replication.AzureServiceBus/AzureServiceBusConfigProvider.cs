using H.Necessaire;

namespace H.Replication.AzureServiceBus
{
    internal class AzureServiceBusConfigProvider
    {
        static readonly Lazy<AzureServiceBusConfig> lazyConfig = new Lazy<AzureServiceBusConfig>(LoadConfigFromEmbeddedSecrets);

        public OperationResult<AzureServiceBusConfig> GetConfig()
        {
            return HSafe.Run(() => lazyConfig.Value);
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
