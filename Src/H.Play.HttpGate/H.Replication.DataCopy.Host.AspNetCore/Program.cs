using H.Replication.DataCopy.Processor;
using H.Replication.AzureServiceBus;
using Microsoft.AspNetCore.Builder;

namespace H.Replication.DataCopy.Host.AspNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder
                .Services
                .AddHReplicationDataCopyProcessor()
                .AddHReplicationAzureServiceBusProcessor()
                ;

            var app = builder.Build();

            //app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
