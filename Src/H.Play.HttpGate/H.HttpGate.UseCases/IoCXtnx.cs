using H.Necessaire;

namespace H.Play.HttpGate.UseCases
{
    public static class IoCXtnx
    {
        public static TDepsReg WithHHttpGateUseCases<TDepsReg>(this TDepsReg deps) where TDepsReg : ImADependencyRegistry
        {
            deps.Register<DependencyGroup>(() => new DependencyGroup());
            return deps;
        }
    }
}
