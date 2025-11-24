using H.Necessaire;
using H.Replication.Contracts.DataContracts;
using MongoDB.Driver;

namespace H.Replication.MongoDB
{
    internal class HMongoSession : IDisposable
    {
        public HMongoSession(MongoDbConfig config)
        {
            MongoAPI = new MongoClient(config.ConnectionString);
            ReplicationRegistryDatabase = MongoAPI.GetDatabase(config.ReplicationRegistryDatabaseName);
            ReplicationRegistryCollection = ReplicationRegistryDatabase.GetCollection<HReplicationRegistryEntry>(config.ReplicationRegistryCollectionName);
        }

        public IMongoClient MongoAPI { get; }
        public IMongoDatabase ReplicationRegistryDatabase { get; }
        public IMongoCollection<HReplicationRegistryEntry> ReplicationRegistryCollection { get; }

        public void Dispose()
        {
            HSafe.Run(() => MongoAPI?.Dispose());
        }

        public static implicit operator HMongoSession(MongoDbConfig config) => new HMongoSession(config);
    }
}
