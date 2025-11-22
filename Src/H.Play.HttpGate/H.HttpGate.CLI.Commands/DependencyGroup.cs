using H.HttpGate.Testicles.AzureTableStorage;
using H.Necessaire;
using H.Play.HttpGate.UseCases;

namespace H.HttpGate.CLI.Commands
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .WithHHttpGateTesticlesAzureTableStorage()
                .WithHHttpGateUseCases()
                .RegisterAlwaysNew<DebugCommand>(() => new DebugCommand())
                ;
        }
    }
}
