using H.Necessaire;

namespace H.HttpGate.Testicles.AzureTableStorage
{
    internal class AzureStorageServiceConfigProvider
    {
        static readonly Lazy<AzureStorageServiceConfig> lazyAzureStorageServiceConfig = new Lazy<AzureStorageServiceConfig>(LoadAzureStorageServiceConfigFromEbeddedSecrets);
        public async Task<OperationResult<AzureStorageServiceConfig>> GetTableStorageConfig()
        {
            return HSafe.Run(() => lazyAzureStorageServiceConfig.Value);
        }

        static AzureStorageServiceConfig LoadAzureStorageServiceConfigFromEbeddedSecrets()
        {
            return
                new AzureStorageServiceConfig
                {
                    AccountName = "AzureStorageAccountName.cfg.txt".ReadSecretFromEmbeddedResources()?.Trim(),
                    AccountKeys = "AzureStorageAccountKeys.cfg.txt".ReadSecretFromEmbeddedResources()?.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)?.ToNonEmptyArray(),
                    AccountConnectionStrings= "AzureStorageAccountConnectionStrings.cfg.txt".ReadSecretFromEmbeddedResources()?.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)?.ToNonEmptyArray(),
                    TableServiceEndpoint = "AzureStorageAccountTableServiceEndpoint.cfg.txt".ReadSecretFromEmbeddedResources()?.Trim(),
                    BlobServiceEndpoint = "AzureStorageAccountBlobServiceEndpoint.cfg.txt".ReadSecretFromEmbeddedResources()?.Trim(),
                    QueueServiceEndpoint = "AzureStorageAccountQueueServiceEndpoint.cfg.txt".ReadSecretFromEmbeddedResources()?.Trim(),
                    FileServiceEndpoint = "AzureStorageAccountFileServiceEndpoint.cfg.txt".ReadSecretFromEmbeddedResources()?.Trim(),
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
