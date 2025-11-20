using H.Necessaire;

namespace H.HttpGate.CLI.Commands
{
    public static class IoCXtnx
    {
        public static TDepsReg WithHHttpGateCliCommands<TDepsReg>(this TDepsReg deps) where TDepsReg : ImADependencyRegistry
        {
            deps.Register<DependencyGroup>(() => new DependencyGroup());
            return deps;
        }
    }
}
