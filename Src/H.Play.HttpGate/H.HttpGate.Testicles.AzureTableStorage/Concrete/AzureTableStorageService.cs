using Azure.Data.Tables;
using H.HttpGate.Testicles.AzureTableStorage.DataContracts;
using H.HttpGate.Testicles.AzureTableStorage.DummyData;
using H.Necessaire;

namespace H.HttpGate.Testicles.AzureTableStorage.Concrete
{
    internal class AzureTableStorageService : IDebug, ImADependency
    {
        static readonly string tableName = typeof(HGateTableStorageTestData).Name;
        AzureStorageServiceConfigProvider azureStorageServiceConfigProvider;
        ImALogger log;
        DummyDataGenerator dummyDataGenerator;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            azureStorageServiceConfigProvider = dependencyProvider.Get<AzureStorageServiceConfigProvider>();
            log = dependencyProvider.GetLogger<AzureTableStorageService>("H.HttpGate.Testicles.AzureTableStorage");
            dummyDataGenerator = dependencyProvider.Get<DummyDataGenerator>();
        }

        public async Task Debug()
        {
            if (!(await azureStorageServiceConfigProvider.GetTableStorageConfig().LogError(log, $"Get Azure Table Storage Config")).RefPayload(out var config))
                return;

            string azureStorgeAccountName = config?.AccountName;
            if (azureStorgeAccountName.IsEmpty())
                return;

            //string azureTableStorageEndpoint = config?.TableServiceEndpoint;
            string azureTableStorageEndpoint = "http://pgdanhsa.dev.localhost:5066/";
            if (azureTableStorageEndpoint.IsEmpty())
                return;

            string azureStorageKey = config?.AccountKeys?.FirstOrDefault();
            if (azureStorageKey.IsEmpty())
                return;

            TableSharedKeyCredential azureStorageCredential = new(azureStorgeAccountName, azureStorageKey);

            TableServiceClient tableServiceClient = new TableServiceClient(new Uri(azureTableStorageEndpoint), azureStorageCredential, new TableClientOptions().And(opts =>
            {
                opts.Retry.MaxRetries = 0;
            }));

            TableClient tableClient = tableServiceClient.GetTableClient(tableName);

            await tableClient.CreateIfNotExistsAsync();

            HGateTableStorageTestData data = dummyDataGenerator.NewHGateTableStorageTestData();

            await tableClient.UpsertEntityAsync(data, mode: TableUpdateMode.Replace);
        }
    }
}
