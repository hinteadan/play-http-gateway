using H.Necessaire;
using H.Necessaire.CLI.Commands;
using System.Reflection;

namespace H.HttpGate.CLI.Commands
{
    internal class DebugCommand : CommandBase
    {
        static readonly Lazy<Assembly[]> lazyHHttpGateAssemblies = new Lazy<Assembly[]>(GetHHttpGateAssemblies);
        Func<string, IDebug> debugProvider;
        public override void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            base.ReferDependencies(dependencyProvider);
            debugProvider = id => id.MorphIfNotEmpty(id => dependencyProvider.Build<IDebug>(id, null, lazyHHttpGateAssemblies.Value));
        }

        public override async Task<OperationResult> Run()
        {
            if (!(await HSafe.Run(async () => (await GetArguments()).Jump(1)?.FirstOrDefault().ID)).Ref(out var debuggerKeyRes, out var debuggerKey))
                return debuggerKeyRes;

            if ((debugProvider?.Invoke(debuggerKey)).RefTo(out var debugger) is null)
                return $"Cannot find any debugger identified by: {debuggerKey.IfEmpty("<<NULL or EMPTY>>")}";

            return await HSafe.Run(debugger.Debug);
        }

        static Assembly[] GetHHttpGateAssemblies()
        {
            return
                AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(a => a?.GetName()?.Name?.StartsWith("H.HttpGate") == true)
                .ToArray()
                ;
        }
    }
}
