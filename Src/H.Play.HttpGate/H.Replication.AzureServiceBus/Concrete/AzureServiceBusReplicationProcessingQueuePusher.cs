using Azure.Messaging.ServiceBus;
using H.Necessaire;
using H.Necessaire.Serialization;
using H.Replication.Contracts;
using H.Replication.Contracts.DataContracts;

namespace H.Replication.AzureServiceBus.Concrete
{
    internal class AzureServiceBusReplicationProcessingQueuePusher : ImAnHReplicationProcessingQueuePusher
    {
        #region Construct
        static readonly ServiceBusClientOptions defaultAzureServiceBusClientOptions = new ServiceBusClientOptions
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets,
            Identifier = "H.Replication.AzureServiceBus.AzureServiceBusReplicationProcessingQueuePusher",
        };
        static readonly ServiceBusSenderOptions defaultAzureServiceBusSenderOptions = new ServiceBusSenderOptions
        {
            Identifier = "H.Replication.AzureServiceBus.AzureServiceBusReplicationProcessingQueuePusher",
        };
        readonly ImALogger log;
        readonly AzureServiceBusConfigProvider configProvider;
        public AzureServiceBusReplicationProcessingQueuePusher(AzureServiceBusConfigProvider configProvider)
        {
            this.log = HApp.Deps.GetLogger<AzureServiceBusReplicationProcessingQueuePusher>("H.Replication.AzureServiceBus");
            this.configProvider = configProvider;
        }
        #endregion

        public async Task<OperationResult<HReplicationProcessingQueueEntry>> Enqueue(HReplicationRegistryEntry replicationRegistryEntry)
        {
            if (!(await configProvider.GetConfig().LogError(log, "configProvider.GetConfig()")).Ref(out var cfgRes, out var config))
                return cfgRes.WithoutPayload<HReplicationProcessingQueueEntry>();

            return
                await HSafe.Run(async () =>
                {

                    string connString = config.ConnectionStrings.First();
                    string queueName = config.ReplicationQueueName;

                    await using ServiceBusClient client = new ServiceBusClient(connString, defaultAzureServiceBusClientOptions);
                    await using ServiceBusSender sender = client.CreateSender(queueName, defaultAzureServiceBusSenderOptions);

                    using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

                    HReplicationProcessingQueueEntry processingQueueEntry = replicationRegistryEntry;

                    bool isAddedToBatch = messageBatch.TryAddMessage(new ServiceBusMessage(processingQueueEntry.ToJsonObject()));

                    await sender.SendMessagesAsync(messageBatch);

                    return processingQueueEntry;

                }).LogError(log, "Enqueue(HReplicationRegistryEntry replicationRegistryEntry)");
        }
    }
}
