using System.Collections.Generic;
using System.Threading.Tasks;

namespace H.HttpGate.Contracts.Public
{
    public interface ImAnHsHttpGateActionRegistry
    {
        Task<IEnumerable<ImAnHsHttpGateAction>> StreamAllKnownActions();
    }
}
