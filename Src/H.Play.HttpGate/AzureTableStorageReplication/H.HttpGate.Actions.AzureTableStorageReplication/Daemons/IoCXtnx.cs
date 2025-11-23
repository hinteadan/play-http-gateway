using Microsoft.Extensions.DependencyInjection;

namespace H.HttpGate.Actions.AzureTableStorageReplication.Daemons
{
    internal static class IoCXtnx
    {
        public static IServiceCollection AddDaemons(this IServiceCollection services)
        {
            services.AddHostedService<AzureTableStorageReplicationProcessingDaemon>();

            return services;
        }
    }
}
