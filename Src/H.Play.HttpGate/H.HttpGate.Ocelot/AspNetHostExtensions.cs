using H.HttpGate.Actions;
using H.HttpGate.Actions.AzureTableStorageReplication;
using H.HttpGate.Core;
using H.Replication.MongoDB;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace H.HttpGate.Ocelot
{
    internal static class AspNetHostExtensions
    {
        public static THostAppBuilder AddHHttpGateOcelot<THostAppBuilder>(this THostAppBuilder builder) where THostAppBuilder : IHostApplicationBuilder
        {
            DirectoryInfo contentDir = new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "..", "H.HttpGate.Ocelot"));
            IEnumerable<FileInfo> ocelotConfigFiles = contentDir.EnumerateFiles("ocelot*.json", SearchOption.TopDirectoryOnly);
            foreach (FileInfo ocelotConfigFile in ocelotConfigFiles)
            {
                ocelotConfigFile.CopyTo(Path.Combine(builder.Environment.ContentRootPath, ocelotConfigFile.Name), overwrite: true);
            }

            builder
                .Services
                .AddHHttpGateCoreServices()
                .AddHHttpGateActionsServices()
                .AddHHttpGateActionsAzureTableStorageReplicationServices()
                .AddHReplicationMongoDBCore()
                .AddSingleton<HHttpGateInvoker>()
                ;

            builder
                .Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("ocelot.json")
                .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json")
                ;

            builder.Services
                .AddOcelot(builder.Configuration)
                ;

            return builder;
        }

        public static async Task<TAppBuilder> UseHHttpGateOcelot<TAppBuilder>(this TAppBuilder app) where TAppBuilder : IApplicationBuilder
        {
            app.Use((ctx, next) => { ctx.Request.EnableBuffering(); return next(); });

            await app.UseOcelot(new OcelotPipelineConfiguration().Configure(app.ApplicationServices));

            return app;
        }
    }
}
