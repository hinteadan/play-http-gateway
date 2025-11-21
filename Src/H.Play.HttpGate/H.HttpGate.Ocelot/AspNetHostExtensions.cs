using H.HttpGate.Actions;
using H.HttpGate.Core;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace H.HttpGate.Ocelot
{
    internal static class AspNetHostExtensions
    {
        public static THostAppBuilder AddHHttpGateOcelot<THostAppBuilder>(this THostAppBuilder builder) where THostAppBuilder : IHostApplicationBuilder
        {
            File.Copy(
                Path.Combine(builder.Environment.ContentRootPath, "..", "H.HttpGate.Ocelot", "ocelot.json"),
                Path.Combine(builder.Environment.ContentRootPath, "ocelot.json"),
                overwrite: true
            );

            builder
                .Services
                .AddHHttpGateCoreServices()
                .AddHHttpGateActionsServices()
                .AddSingleton<HHttpGateInvoker>()
                ;

            builder
                .Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddOcelot()
                ;

            builder.Services
                .AddOcelot(builder.Configuration)
                ;

            return builder;
        }

        public static async Task<TAppBuilder> UseHHttpGateOcelot<TAppBuilder>(this TAppBuilder app) where TAppBuilder : IApplicationBuilder
        {
            await app.UseOcelot(new OcelotPipelineConfiguration().Configure(app.ApplicationServices));

            return app;
        }
    }
}
