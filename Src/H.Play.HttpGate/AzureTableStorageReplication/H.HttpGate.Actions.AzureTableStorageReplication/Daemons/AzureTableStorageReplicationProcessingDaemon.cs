using H.HttpGate.Actions.AzureTableStorageReplication.BLL;
using H.Necessaire;
using Microsoft.Extensions.Hosting;

namespace H.HttpGate.Actions.AzureTableStorageReplication.Daemons
{
    internal class AzureTableStorageReplicationProcessingDaemon : BackgroundService
    {
        #region Construct
        static readonly TimeSpan processingStartDelay = TimeSpan.FromSeconds(5);
        static readonly TimeSpan processingInterval = TimeSpan.FromSeconds(5);
        readonly ImAPeriodicAction periodicProcessingAction = HApp.Deps.Get<ImAPeriodicAction>();
        readonly AzureTableStorageReplicator azureTableStorageReplicator;
        readonly ImALogger log;
        public AzureTableStorageReplicationProcessingDaemon(AzureTableStorageReplicator azureTableStorageReplicator)
        {
            this.azureTableStorageReplicator = azureTableStorageReplicator;
            log = HApp.Deps.GetLogger<AzureTableStorageReplicationProcessingDaemon>("H.HttpGate.Actions.AzureTableStorageReplication");
        }
        #endregion

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(async () =>
            {
                periodicProcessingAction.Stop();
                await RunProcessingSession(stoppingToken);
            });

            periodicProcessingAction.StartDelayed(processingStartDelay, processingInterval, async () => await RunProcessingSession(stoppingToken));
        }

        async Task RunProcessingSession(CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested)
            {
                await HSafe.Run(azureTableStorageReplicator.ShutdownASAP).LogError(log, "azureTableStorageReplicator.ShutdownASAP");
                return;
            }

            await HSafe.Run(async () => await azureTableStorageReplicator.RunReplicationSession(cancelToken)).LogError(log, "azureTableStorageReplicator.RunReplicationSession");
        }
    }
}
