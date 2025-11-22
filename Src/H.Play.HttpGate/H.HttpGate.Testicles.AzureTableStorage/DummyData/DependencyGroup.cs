using H.Necessaire;

namespace H.HttpGate.Testicles.AzureTableStorage.DummyData
{
    internal class DependencyGroup : ImADependencyGroup
    {
        public void RegisterDependencies(ImADependencyRegistry dependencyRegistry)
        {
            dependencyRegistry
                .Register<DummyDataGenerator>(() => new DummyDataGenerator())
                ;
        }
    }
}
