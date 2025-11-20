using H.Necessaire;

namespace H.Play.HttpGate.UseCases
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .RegisterAlwaysNew<DebugUseCase>(() => new DebugUseCase())
                ;
        }
    }
}
