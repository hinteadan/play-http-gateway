using H.HttpGate.Core.Concrete;
using H.Necessaire;
using Microsoft.Extensions.DependencyInjection;

namespace H.HttpGate.Core
{
    public static class IoCXtnx
    {
        public static IServiceCollection AddHHttpGateCoreServices(this IServiceCollection services)
        {
            return
                services
                .AddTransient<Func<Type, ImALogger>>(sp => t => HGateApp.DependencyRegistry.GetLogger(t, "H.HttpGate"))
                .AddHHttpGateCoreConcretes()
                ;
        }
    }
}
