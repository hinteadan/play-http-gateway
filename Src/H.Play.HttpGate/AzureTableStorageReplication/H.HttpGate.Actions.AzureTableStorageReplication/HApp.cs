using H.Necessaire;

namespace H.HttpGate.Actions.AzureTableStorageReplication
{
    internal static class HApp
    {
        static readonly ImADependencyRegistry dependencyRegistry = IoC.NewDependencyRegistry().Register<HNecessaireDependencyGroup>(() => new HNecessaireDependencyGroup());

        public static ImADependencyRegistry Deps => dependencyRegistry;
    }
}
