using H.Necessaire;

namespace H.HttpGate.Testicles.AzureTableStorage.Concrete
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .Register<AzureTableStorageService>(() => new AzureTableStorageService())
                ;
        }
    }
}
