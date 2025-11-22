using H.Necessaire;

namespace H.HttpGate.Testicles.AzureTableStorage
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .Register<AzureStorageServiceConfigProvider>(() => new AzureStorageServiceConfigProvider())
                .Register<Concrete.DependencyGroup>(() => new Concrete.DependencyGroup())
                .Register<DummyData.DependencyGroup>(() => new DummyData.DependencyGroup())
                ;
        }
    }
}
