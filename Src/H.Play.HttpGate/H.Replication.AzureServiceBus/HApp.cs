using H.Necessaire;

namespace H.Replication.AzureServiceBus
{
    internal static class HApp
    {
        static readonly ImADependencyRegistry dependencyRegistry = IoC.NewDependencyRegistry().Register<HNecessaireDependencyGroup>(() => new HNecessaireDependencyGroup());

        public static ImADependencyRegistry Deps => dependencyRegistry;
    }
}
