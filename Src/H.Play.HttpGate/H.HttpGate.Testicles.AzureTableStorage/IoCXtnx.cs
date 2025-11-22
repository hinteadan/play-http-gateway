using H.Necessaire;

namespace H.HttpGate.Testicles.AzureTableStorage
{
    public static class IoCXtnx
    {
        public static TDepsReg WithHHttpGateTesticlesAzureTableStorage<TDepsReg>(this TDepsReg deps) where TDepsReg : ImADependencyRegistry
        {
            deps.Register<DependencyGroup>(() => new DependencyGroup());
            return deps;
        }
    }
}
