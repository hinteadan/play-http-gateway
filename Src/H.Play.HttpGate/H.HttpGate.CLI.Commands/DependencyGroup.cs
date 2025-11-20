using H.Necessaire;
using H.Play.HttpGate.UseCases;

namespace H.HttpGate.CLI.Commands
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .WithHHttpGateUseCases()
                .RegisterAlwaysNew<DebugCommand>(() => new DebugCommand())
                ;
        }
    }
}
