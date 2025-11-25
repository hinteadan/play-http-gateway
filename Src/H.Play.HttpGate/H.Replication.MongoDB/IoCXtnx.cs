using H.Necessaire;
using H.Replication.Contracts;
using H.Replication.Contracts.DataContracts;
using H.Replication.MongoDB.Concrete;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace H.Replication.MongoDB
{
    public static class IoCXtnx
    {
        public static IServiceCollection AddHReplicationMongoDB(this IServiceCollection services)
        {
            ConfigMongoDefaults();

            return
                services
                .AddSingleton<MongoDbConfigProvider>()
                .AddSingleton<MongoDbInteractor>()
                .AddSingleton<ImAnHReplicationRegistry, MongoDbReplicationRegistry>()
                .AddSingleton<ImAnHReplicationRequestProcessor, MongoDbReplicationRequestProcessor>()
                ;
        }

        public static TDepsReg WithHReplicationMongoDB<TDepsReg>(this TDepsReg deps) where TDepsReg : ImADependencyRegistry
        {
            ConfigMongoDefaults();

            deps
                .Register<MongoDbConfigProvider>(() => new MongoDbConfigProvider())
                .Register<MongoDbInteractor>(() => new MongoDbInteractor(deps.Get<MongoDbConfigProvider>()))
                .Register<ImAnHReplicationRegistry>(() => new MongoDbReplicationRegistry(deps.Get<MongoDbInteractor>()))
                .Register<ImAnHReplicationRequestProcessor>(() => new MongoDbReplicationRequestProcessor(deps.Get<MongoDbInteractor>()))
                .Register<MongoDbDebugger>(() => new MongoDbDebugger())
                ;
            return deps;
        }

        static bool isMongoDefaultsConfigured = false;
        static readonly object configMongoDefaultsLocker = new object();
        static void ConfigMongoDefaults()
        {
            lock(configMongoDefaultsLocker)
            {
                if (isMongoDefaultsConfigured)
                    return;

                BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
                BsonClassMap.RegisterClassMap<HReplicationRegistryEntry>(cfg => {
                    cfg.AutoMap();
                    cfg.SetIgnoreExtraElements(true);
                });

                isMongoDefaultsConfigured = true;
            }
        }
    }
}
