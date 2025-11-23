using H.HttpGate.Actions.AzureTableStorageReplication.BLL;
using H.HttpGate.Actions.AzureTableStorageReplication.Daemons;
using H.HttpGate.Contracts.Public;
using Microsoft.Extensions.DependencyInjection;

namespace H.HttpGate.Actions.AzureTableStorageReplication
{
    public static class IoCXtnx
    {
        public static IServiceCollection AddHHttpGateActionsAzureTableStorageReplicationServices(this IServiceCollection services)
        {
            return
                services
                .AddBLL()
                .AddDaemons()
                .AddSingleton<ImAnHsHttpGateAction, AzureTableStorageReplicationAction>()
                ;
        }
    }
}
