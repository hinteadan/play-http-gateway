using H.HttpGate.Contracts.Public;
using H.HttpGate.Contracts.Public.DataContracts;
using H.Necessaire;

namespace H.HttpGate.Actions
{
    internal class DebugAction : ImAnHsHttpGateAction
    {
        public async Task<OperationResult> Run(HHttpGateResponse response)
        {
            return true;
        }
    }
}