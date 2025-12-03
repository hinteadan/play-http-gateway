# Runtime Configuration

Runtime configuration is grabbed like below, with fallback, in order of priority:

1. Runtime Host Configuration _(a.k.a. `IConfiguration` ~~ `Environment` vars, `appSettings.json`, etc.)_
1. _OR_ Embedded Resource as `Secrets/*.cfg.txt` files under each respective assembly 

---

## Modules that require config secrets

### `H.Replication.AzureServiceBus`

1. `IConfiguration` / `appSettings.json` config:
    1. Root Section Path: `HReplication`/`AzureServiceBus`
    1. Keys
        1. `Keys` - array of strings
        1. `ConnectionStrings` - array of strings
        1. `ReplicationQueueName` - string
1. _OR_ Embedded Resource as `Secrets/*.cfg.txt` files
    1. `AzureSBConnectionStrings.cfg.txt` - can be multivalue, each value per new line
    1. `AzureSBKeys.cfg.txt` - can be multivalue, each value per new line
    1. `AzureSBReplicationProcessingQueueName.cfg.txt` - single value, one liner

### `H.Replication.MongoDB`
1. `IConfiguration` / `appSettings.json` config:
    1. Root Section Path: `HReplication`/`MongoDB`
    1. Keys
        1. `ConnectionString` - string
        1. `ReplicationRegistryDatabaseName` - string
        1. `ReplicationRegistryCollectionName` - string
1. _OR_ Embedded Resource as `Secrets/*.cfg.txt` files
    1. `MongoDbConnectionString.cfg.txt` - single value, one liner
    1. `MongoDbReplicationRegistryCollectionName.cfg.txt` - single value, one liner
    1. `MongoDbReplicationRegistryDatabaseName.cfg.txt` - single value, one liner

### `H.HttpGate.Testicles.AzureTableStorage`
1. `IConfiguration` / `appSettings.json` config:
    1. Root Section Path: `HReplication`/`AzureStorage`
    1. Keys
        1. `AccountName` - string
        1. `AccountKeys` - can be multivalue, each value per new line
        1. `AccountConnectionStrings` - can be multivalue, each value per new line
        1. `TableServiceEndpoint` - string
        1. `BlobServiceEndpoint` - string
        1. `QueueServiceEndpoint` - string
        1. `FileServiceEndpoint` - string
1. _OR_ Embedded Resource as `Secrets/*.cfg.txt` files
    1. `AzureStorageAccountBlobServiceEndpoint.cfg.txt` - single value, one liner
    1. `AzureStorageAccountConnectionStrings.cfg.txt` - can be multivalue, each value per new line
    1. `AzureStorageAccountFileServiceEndpoint.cfg.txt` - single value, one liner
    1. `AzureStorageAccountKeys.cfg.txt` - can be multivalue, each value per new line
    1. `AzureStorageAccountName.cfg.txt` - single value, one liner
    1. `AzureStorageAccountQueueServiceEndpoint.cfg.txt` - single value, one liner
    1. `AzureStorageAccountTableServiceEndpoint.cfg.txt` - single value, one liner