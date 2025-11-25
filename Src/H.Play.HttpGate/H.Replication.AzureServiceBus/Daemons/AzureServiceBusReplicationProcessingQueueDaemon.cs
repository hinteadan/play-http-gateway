using Azure.Messaging.ServiceBus;
using H.Necessaire;
using H.Replication.Contracts;
using H.Replication.DataCopy.Processor;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace H.Replication.AzureServiceBus.Daemons
{
    internal class AzureServiceBusReplicationProcessingQueueDaemon : IHostedService
    {
        #region Construct
        static readonly ServiceBusClientOptions defaultAzureServiceBusClientOptions = new ServiceBusClientOptions
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets,
            Identifier = "H.Replication.AzureServiceBus.AzureServiceBusReplicationProcessingQueueDaemon",
        };
        static readonly ServiceBusProcessorOptions defaultAzureServiceBusProcessorOptions = new ServiceBusProcessorOptions
        {
            Identifier = "H.Replication.AzureServiceBus.AzureServiceBusReplicationProcessingQueueDaemon",
        };
        readonly AzureServiceBusConfigProvider configProvider;
        readonly ImAnHReplicationProcessingQueueProcessor replicationProcessingQueueProcessor;
        readonly ImALogger log;
        public AzureServiceBusReplicationProcessingQueueDaemon(AzureServiceBusConfigProvider configProvider, ImAnHReplicationProcessingQueueProcessor replicationProcessingQueueProcessor)
        {
            this.configProvider = configProvider;
            this.replicationProcessingQueueProcessor = replicationProcessingQueueProcessor;
            log = HApp.Deps.GetLogger<AzureServiceBusReplicationProcessingQueueDaemon>("H.Replication.AzureServiceBus");
        }
        #endregion

        ServiceBusClient client = null;
        ServiceBusProcessor processor = null;
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (!(await configProvider.GetConfig().LogError(log, "configProvider.GetConfig()")).Ref(out var cfgRes, out var config))
                return;

            await HSafe.Run(async () =>
            {

                string connString = config.ConnectionStrings.First();
                string queueName = config.ReplicationQueueName;

                client = new ServiceBusClient(connString, defaultAzureServiceBusClientOptions);
                processor = client.CreateProcessor(queueName, defaultAzureServiceBusProcessorOptions);

                processor.ProcessMessageAsync += Processor_ProcessMessageAsync;
                processor.ProcessErrorAsync += Processor_ProcessErrorAsync;

                await processor.StartProcessingAsync();

            }).LogError(log, "Start Azure Service Bus Queue Processing Daemon");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (processor != null)
            {
                await HSafe.Run(async () => await processor.StopProcessingAsync());
                await HSafe.Run(async () => await processor.DisposeAsync());
            }

            if (client != null)
            {
                await HSafe.Run(async () => await client.DisposeAsync());
            }
        }

        async Task Processor_ProcessErrorAsync(ProcessErrorEventArgs processErrorEventArgs)
        {
            await log.LogError($"Error processing message {processErrorEventArgs.Identifier}", processErrorEventArgs.Exception);
        }

        async Task Processor_ProcessMessageAsync(ProcessMessageEventArgs processMessageEventArgs)
        {
            if (!(await replicationProcessingQueueProcessor.Process(processMessageEventArgs.Message?.Body?.ToArray()).LogError(log, "replicationProcessingQueueProcessor.Process")).Ref(out var res))
            {
                await processMessageEventArgs.DeadLetterMessageAsync(processMessageEventArgs.Message, res.Reason, string.Join(",", [..res.FlattenReasons() ?? [], ..res.Comments ?? []]).NullIfEmpty()?.EllipsizeIfNecessary(4096));
                return;
            }

            await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
        }
    }
}
