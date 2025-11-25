using H.Replication.AzureServiceBus;
using H.Replication.DataCopy.Processor;
using Microsoft.Extensions.Hosting;

namespace H.Replication.DataCopy.Host.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder(args);

            builder
                .Services
                .AddHReplicationDataCopyProcessor()
                .AddHReplicationAzureServiceBusProcessor()
                ;

            IHost host = builder.Build();

            await host.RunAsync();
        }
    }
}
