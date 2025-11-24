using H.Necessaire;
using H.Replication.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace H.Replication.MongoDB
{
    public static class IoCXtnx
    {
        public static IServiceCollection AddHReplicationMongoDBCore(this IServiceCollection services)
        {
            return
                services
                .AddSingleton<MongoDbConfigProvider>()
                .AddSingleton<MongoDbInteractor>()
                .AddSingleton<ImAnHReplicationRegistry, Concrete.MongoDbReplicationRegistry>()
                ;
        }

        public static TDepsReg WithHReplicationMongoDBCore<TDepsReg>(this TDepsReg deps) where TDepsReg : ImADependencyRegistry
        {
            deps
                .Register<MongoDbConfigProvider>(() => new MongoDbConfigProvider())
                .Register<MongoDbInteractor>(() => new MongoDbInteractor(deps.Get<MongoDbConfigProvider>()))
                .Register<ImAnHReplicationRegistry>(() => new Concrete.MongoDbReplicationRegistry(deps.Get<MongoDbInteractor>()))
                .Register<MongoDbDebugger>(() => new MongoDbDebugger())
                ;
            return deps;
        }
    }
}
