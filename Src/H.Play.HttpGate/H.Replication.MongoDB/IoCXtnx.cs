using H.Necessaire;
using H.Replication.Contracts;
using H.Replication.MongoDB.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace H.Replication.MongoDB
{
    public static class IoCXtnx
    {
        public static IServiceCollection AddHReplicationMongoDB(this IServiceCollection services)
        {
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
            deps
                .Register<MongoDbConfigProvider>(() => new MongoDbConfigProvider())
                .Register<MongoDbInteractor>(() => new MongoDbInteractor(deps.Get<MongoDbConfigProvider>()))
                .Register<ImAnHReplicationRegistry>(() => new MongoDbReplicationRegistry(deps.Get<MongoDbInteractor>()))
                .Register<ImAnHReplicationRequestProcessor>(() => new MongoDbReplicationRequestProcessor(deps.Get<MongoDbInteractor>()))
                .Register<MongoDbDebugger>(() => new MongoDbDebugger())
                ;
            return deps;
        }
    }
}
