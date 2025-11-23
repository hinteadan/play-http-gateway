using Microsoft.Extensions.DependencyInjection;

namespace H.HttpGate.Actions.AzureTableStorageReplication.BLL
{
    internal static class IoCXtnx
    {
        public static IServiceCollection AddBLL(this IServiceCollection services)
        {
            services.AddSingleton<AzureTableStorageReplicator>();

            return services;
        }
    }
}
