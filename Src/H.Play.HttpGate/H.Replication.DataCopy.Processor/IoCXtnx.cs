using H.Replication.Contracts;
using H.Replication.MongoDB;
using Microsoft.Extensions.DependencyInjection;

namespace H.Replication.DataCopy.Processor
{
    public static class IoCXtnx
    {
        public static IServiceCollection AddHReplicationDataCopyProcessor(this IServiceCollection services)
        {
            return
                services
                .AddHReplicationMongoDB()
                .AddSingleton<ImAnHReplicationProcessingQueueProcessor, HReplicationProcessingQueueProcessor>()
                ;
        }
    }
}
