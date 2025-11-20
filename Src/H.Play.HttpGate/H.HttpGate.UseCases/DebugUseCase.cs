using H.Necessaire;
using H.Necessaire.Runtime;

namespace H.Play.HttpGate.UseCases
{
    internal class DebugUseCase : UseCaseBase, IDebug
    {
        public Task Debug()
        {
            return Task.CompletedTask;
        }
    }
}
