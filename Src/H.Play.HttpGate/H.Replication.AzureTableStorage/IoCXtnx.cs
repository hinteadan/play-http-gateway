using H.Replication.Core;
using Microsoft.Extensions.DependencyInjection;

namespace H.Replication.AzureTableStorage
{
    public static class IoCXtnx
    {
        public static IServiceCollection AddAzureTableStorageReplication(this IServiceCollection services)
        {
            return
                services
                .AddHReplicationCore()
                ;
        }
    }
}
