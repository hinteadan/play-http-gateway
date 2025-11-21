using H.Necessaire;

namespace H.HttpGate.Core
{
    internal static class HGateApp
    {
        static readonly ImADependencyRegistry dependencyRegistry = IoC.NewDependencyRegistry().Register<HNecessaireDependencyGroup>(() => new HNecessaireDependencyGroup());
        public static ImADependencyRegistry DependencyRegistry => dependencyRegistry;
    }
}
