using H.Config;
using H.Necessaire;
using Microsoft.Extensions.Configuration;

namespace H.HttpGate.Testicles.AzureTableStorage
{
    internal class AzureStorageServiceConfigProvider
    {
        #region Construct
        readonly Lazy<AzureStorageServiceConfig> lazyConfig;
        readonly IConfiguration configuration;
        public AzureStorageServiceConfigProvider(IConfiguration configuration = null)
        {
            this.configuration = configuration;
            lazyConfig = new Lazy<AzureStorageServiceConfig>(LoadConfig);
        }
        #endregion

        public async Task<OperationResult<AzureStorageServiceConfig>> GetTableStorageConfig()
        {
            return HSafe.Run(() => lazyConfig.Value);
        }

        AzureStorageServiceConfig LoadConfig()
        {
            return
                configuration.GetConfigSection("HReplication", "AzureStorage").MorphIfNotNull(x => new AzureStorageServiceConfig
                {
                    AccountName = x.GetConfigValue(nameof(AzureStorageServiceConfig.AccountName)),
                    AccountKeys = x.GetConfigValues(nameof(AzureStorageServiceConfig.AccountKeys)),
                    AccountConnectionStrings = x.GetConfigValues(nameof(AzureStorageServiceConfig.AccountConnectionStrings)),
                    TableServiceEndpoint = x.GetConfigValue(nameof(AzureStorageServiceConfig.TableServiceEndpoint)),
                    BlobServiceEndpoint = x.GetConfigValue(nameof(AzureStorageServiceConfig.BlobServiceEndpoint)),
                    QueueServiceEndpoint = x.GetConfigValue(nameof(AzureStorageServiceConfig.QueueServiceEndpoint)),
                    FileServiceEndpoint = x.GetConfigValue(nameof(AzureStorageServiceConfig.FileServiceEndpoint)),
                })
                ??
                new AzureStorageServiceConfig
                {
                    AccountName = "AzureStorageAccountName.cfg.txt".ReadSecretFromEmbeddedResources(typeof(AzureStorageServiceConfigProvider).Assembly)?.Trim(),
                    AccountKeys = "AzureStorageAccountKeys.cfg.txt".ReadSecretFromEmbeddedResources(typeof(AzureStorageServiceConfigProvider).Assembly)?.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)?.ToNonEmptyArray(),
                    AccountConnectionStrings = "AzureStorageAccountConnectionStrings.cfg.txt".ReadSecretFromEmbeddedResources(typeof(AzureStorageServiceConfigProvider).Assembly)?.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)?.ToNonEmptyArray(),
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
