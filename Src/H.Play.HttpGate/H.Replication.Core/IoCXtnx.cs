using H.Replication.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace H.Replication.Core
{
    public static class IoCXtnx
    {
        public static IServiceCollection AddHReplicationCore(this IServiceCollection services)
        {
            return
                services
                .AddSingleton<ImAnHReplicator, HReplicator>()
                ;
        }
    }
}
