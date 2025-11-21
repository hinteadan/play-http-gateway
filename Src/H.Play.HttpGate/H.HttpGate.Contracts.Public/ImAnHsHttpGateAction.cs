using H.HttpGate.Contracts.Public.DataContracts;
using H.Necessaire;
using System.Threading.Tasks;

namespace H.HttpGate.Contracts.Public
{
    public interface ImAnHsHttpGateAction
    {
        Task<OperationResult> Run(HHttpGateResponse response);
    }
}
