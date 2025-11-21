using H.HttpGate.Contracts.Public;

namespace H.HttpGate.Core.Concrete
{
    internal class HsHttpGateActionRegistry : ImAnHsHttpGateActionRegistry
    {
        readonly IEnumerable<ImAnHsHttpGateAction> allServiceRegisteredActions;
        public HsHttpGateActionRegistry(IEnumerable<ImAnHsHttpGateAction> allServiceRegisteredActions)
        {
            this.allServiceRegisteredActions = allServiceRegisteredActions;
        }

        public async Task<IEnumerable<ImAnHsHttpGateAction>> StreamAllKnownActions()
        {
            return allServiceRegisteredActions;
        }
    }
}
