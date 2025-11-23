using H.Necessaire;

namespace H.Replication.Core
{
    internal static class HApp
    {
        static readonly ImADependencyRegistry dependencyRegistry = IoC.NewDependencyRegistry().Register<HNecessaireDependencyGroup>(() => new HNecessaireDependencyGroup());

        public static ImADependencyRegistry Deps => dependencyRegistry;
    }
}
