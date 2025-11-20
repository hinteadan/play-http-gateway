using H.HttpGate.CLI.Commands;
using H.Necessaire;

namespace H.Play.HttpGate.CLI
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .WithHHttpGateCliCommands()
                ;
        }
    }
}
