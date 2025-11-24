using H.Necessaire;

namespace H.Replication.MongoDB
{
    internal static class HApp
    {
        static readonly ImADependencyRegistry dependencyRegistry = IoC.NewDependencyRegistry().Register<HNecessaireDependencyGroup>(() => new HNecessaireDependencyGroup());

        public static ImADependencyRegistry Deps => dependencyRegistry;
    }
}
