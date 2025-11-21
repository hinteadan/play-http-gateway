using Ocelot.Middleware;

namespace H.HttpGate.Ocelot
{
    internal static class OcelotConfigExtensions
    {
        public static OcelotPipelineConfiguration Configure(this OcelotPipelineConfiguration ocelotPipelineConfiguration, IServiceProvider serviceProvider)
        {
            HHttpGateInvoker hHttpGateInvoker = serviceProvider.GetRequiredService<HHttpGateInvoker>();

            ocelotPipelineConfiguration.PreQueryStringBuilderMiddleware = async (httpContext, nextMiddlewareInvoker) => {

                await nextMiddlewareInvoker.Invoke();

                await hHttpGateInvoker.Run(httpContext);

            };

            return ocelotPipelineConfiguration;
        }
    }
}
