using H.Necessaire;
using H.Necessaire.Runtime;
using H.Necessaire.Runtime.ExternalCommandRunner;
using System.Diagnostics;
using System.Threading;

namespace H.Play.HttpGate.UseCases
{
    internal class DebugUseCase : UseCaseBase, IDebug
    {
        ImAnExternalCommandRunner externalCommandRunner;
        public override void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            base.ReferDependencies(dependencyProvider);

            externalCommandRunner = dependencyProvider.Get<ImAnExternalCommandRunner>();
        }

        public async Task Debug()
        {
            StartHttpHost(new CancellationTokenSource().RefTo(out var cancellationTokenSource).Token);

            await Task.Delay(TimeSpan.FromSeconds(10));

            await cancellationTokenSource.CancelAsync();
        }

        void StartHttpHost(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => {

                HSafe.Run(() => {

                    Process hostProcess = Process.GetProcessesByName("H.HttpGate.Runtime.Host.AspNetCore").Single();
                    hostProcess.Kill();

                });

            });

            Task.Run(async () => {

                (await
                    externalCommandRunner
                    .RunCmd(
                        cancellationToken,
                        $"dotnet run --project " +
                        $"C:\\Wrk\\play-http-gateway\\Src\\H.Play.HttpGate\\H.HttpGate.Runtime.Host.AspNetCore\\H.HttpGate.Runtime.Host.AspNetCore.csproj" +
                        $" -c Debug"
                    )
                )
                .ThrowOnFail()
                ;

            }, cancellationToken)
            .DontWait()
            ;
        }
    }
}
