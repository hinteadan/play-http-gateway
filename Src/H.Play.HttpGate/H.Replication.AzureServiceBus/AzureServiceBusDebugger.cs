using Azure.Messaging.ServiceBus;
using H.Necessaire;
using H.Necessaire.Serialization;
using H.Replication.Contracts.DataContracts;

namespace H.Replication.AzureServiceBus
{
    internal class AzureServiceBusDebugger : ImADependency, IDebug
    {
        static readonly ServiceBusClientOptions defaultAzureServiceBusClientOptions = new ServiceBusClientOptions
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets,
            Identifier = "H.Replication.AzureServiceBus.AzureServiceBusDebugger",
        };
        static readonly ServiceBusSenderOptions defaultAzureServiceBusSenderOptions = new ServiceBusSenderOptions
        {
            Identifier = "H.Replication.AzureServiceBus.AzureServiceBusDebugger",
        };
        AzureServiceBusConfigProvider configProvider;
        public void ReferDependencies(ImADependencyProvider dependencyProvider)
        {
            this.configProvider = dependencyProvider.Get<AzureServiceBusConfigProvider>();
        }

        public async Task Debug()
        {
            AzureServiceBusConfig config = configProvider.GetConfig().ThrowOnFailOrReturn();
            string connString = config.ConnectionStrings.First();
            string queueName = config.ReplicationQueueName;

            await using ServiceBusClient client = new ServiceBusClient(connString, defaultAzureServiceBusClientOptions);
            await using ServiceBusSender sender = client.CreateSender(queueName, defaultAzureServiceBusSenderOptions);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            HReplicationProcessingQueueEntry messageContent = new HReplicationProcessingQueueEntry();

            bool isAddedToBatch = messageBatch.TryAddMessage(new ServiceBusMessage(messageContent.ToJsonObject()));

            await sender.SendMessagesAsync(messageBatch);
        }
    }
}
