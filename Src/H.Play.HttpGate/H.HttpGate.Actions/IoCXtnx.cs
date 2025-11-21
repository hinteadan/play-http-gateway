using H.HttpGate.Contracts.Public;
using Microsoft.Extensions.DependencyInjection;

namespace H.HttpGate.Actions
{
    public static class IoCXtnx
    {
        public static IServiceCollection AddHHttpGateActionsServices(this IServiceCollection services)
        {
            return
                services
                .AddSingleton<ImAnHsHttpGateAction, DebugAction>()
                ;
        }
    }
}
