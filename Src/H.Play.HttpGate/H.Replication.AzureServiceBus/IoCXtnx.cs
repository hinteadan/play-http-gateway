using H.Necessaire;
using H.Replication.AzureServiceBus.Concrete;
using H.Replication.AzureServiceBus.Daemons;
using H.Replication.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace H.Replication.AzureServiceBus
{
    public static class IoCXtnx
    {
        public static IServiceCollection AddHReplicationAzureServiceBus(this IServiceCollection services)
        {
            return
                services
                .AddSingleton<AzureServiceBusConfigProvider>()
                .AddSingleton<ImAnHReplicationProcessingQueuePusher, AzureServiceBusReplicationProcessingQueuePusher>()
                ;
        }

        public static IServiceCollection AddHReplicationAzureServiceBusProcessor(this IServiceCollection services)
        {
            return
                services
                .AddSingleton<AzureServiceBusConfigProvider>()
                .AddHostedService<AzureServiceBusReplicationProcessingQueueDaemon>()
                ;
        }

        public static TDepsReg WithHReplicationAzureServiceBus<TDepsReg>(this TDepsReg deps) where TDepsReg : ImADependencyRegistry
        {
            deps
                .Register<AzureServiceBusConfigProvider>(() => new AzureServiceBusConfigProvider())
                .Register<AzureServiceBusDebugger>(() => new AzureServiceBusDebugger())
                ;
            return deps;
        }
    }
}
