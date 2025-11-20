using H.Necessaire.CLI;
using H.Necessaire.Runtime.CLI;

namespace H.Play.HttpGate.CLI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await
                new CliApp()
                .WithEverything()
                .With(x => x.Register<DependencyGroup>(() => new DependencyGroup()))
                .Run()
                ;
        }
    }
}
