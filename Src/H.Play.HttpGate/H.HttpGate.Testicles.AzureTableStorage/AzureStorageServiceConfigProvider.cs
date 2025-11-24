using H.Necessaire;

namespace H.HttpGate.Testicles.AzureTableStorage
{
    internal class AzureStorageServiceConfigProvider
    {
        static readonly Lazy<AzureStorageServiceConfig> lazyAzureStorageServiceConfig = new Lazy<AzureStorageServiceConfig>(LoadAzureStorageServiceConfigFromEmbeddedSecrets);
        public async Task<OperationResult<AzureStorageServiceConfig>> GetTableStorageConfig()
        {
            return HSafe.Run(() => lazyAzureStorageServiceConfig.Value);
        }

        static AzureStorageServiceConfig LoadAzureStorageServiceConfigFromEmbeddedSecrets()
        {
            return
                new AzureStorageServiceConfig
                {
                    AccountName = "AzureStorageAccountName.cfg.txt".ReadSecretFromEmbeddedResources(typeof(AzureStorageServiceConfigProvider).Assembly)?.Trim(),
                    AccountKeys = "AzureStorageAccountKeys.cfg.txt".ReadSecretFromEmbeddedResources(typeof(AzureStorageServiceConfigProvider).Assembly)?.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)?.ToNonEmptyArray(),
                    AccountConnectionStrings= "AzureStorageAccountConnectionStrings.cfg.txt".ReadSecretFromEmbeddedResources(typeof(AzureStorageServiceConfigProvider).Assembly)?.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)?.ToNonEmptyArray(),
                    TableServiceEndpoint = "AzureStorageAccountTableServiceEndpoint.cfg.txt".ReadSecretFromEmbeddedResources(typeof(AzureStorageServiceConfigProvider).Assembly)?.Trim(),
                    BlobServiceEndpoint = "AzureStorageAccountBlobServiceEndpoint.cfg.txt".ReadSecretFromEmbeddedResources(typeof(AzureStorageServiceConfigProvider).Assembly)?.Trim(),
                    QueueServiceEndpoint = "AzureStorageAccountQueueServiceEndpoint.cfg.txt".ReadSecretFromEmbeddedResources(typeof(AzureStorageServiceConfigProvider).Assembly)?.Trim(),
                    FileServiceEndpoint = "AzureStorageAccountFileServiceEndpoint.cfg.txt".ReadSecretFromEmbeddedResources(typeof(AzureStorageServiceConfigProvider).Assembly)?.Trim(),
                }
                ;
        }
    }

    internal class AzureStorageServiceConfig
    {
        public string AccountName { get; set; }
        public string[] AccountKeys { get; set; }
        public string[] AccountConnectionStrings { get; set; }

        public string TableServiceEndpoint { get; set; }
        public string BlobServiceEndpoint { get; set; }
        public string QueueServiceEndpoint { get; set; }
        public string FileServiceEndpoint { get; set; }
    }
}
