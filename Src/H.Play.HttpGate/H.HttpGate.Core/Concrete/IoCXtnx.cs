using H.HttpGate.Contracts.Public;
using Microsoft.Extensions.DependencyInjection;

namespace H.HttpGate.Core.Concrete
{
    internal static class IoCXtnx
    {
        public static IServiceCollection AddHHttpGateCoreConcretes(this IServiceCollection services)
        {
            return
                services
                .AddSingleton<ImAnHsHttpGateActionRegistry, HsHttpGateActionRegistry>()
                ;
        }
    }
}
