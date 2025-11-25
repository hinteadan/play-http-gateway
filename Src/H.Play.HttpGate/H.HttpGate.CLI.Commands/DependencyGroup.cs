using H.HttpGate.Testicles.AzureTableStorage;
using H.Necessaire;
using H.Play.HttpGate.UseCases;
using H.Replication.AzureServiceBus;
using H.Replication.MongoDB;

namespace H.HttpGate.CLI.Commands
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .WithHHttpGateTesticlesAzureTableStorage()
                .WithHHttpGateUseCases()
                .WithHReplicationMongoDB()
                .WithHReplicationAzureServiceBus()
                .RegisterAlwaysNew<DebugCommand>(() => new DebugCommand())
                ;
        }
    }
}
