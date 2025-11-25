using Azure.Messaging.ServiceBus;
using H.Necessaire;
using H.Necessaire.Serialization;
using H.Replication.Contracts;
using H.Replication.Contracts.DataContracts;
using H.Replication.DataCopy.Processor;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace H.Replication.DataCopy.Host.AzureFunctions;

public class HReplicationProcessingQueueProcessorFunction
{
    #region Construct
    readonly ILogger<HReplicationProcessingQueueProcessorFunction> logger;
    readonly ImAnHReplicationProcessingQueueProcessor replicationProcessingQueueProcessor;
    public HReplicationProcessingQueueProcessorFunction(
        ILogger<HReplicationProcessingQueueProcessorFunction> logger,
        ImAnHReplicationProcessingQueueProcessor replicationProcessingQueueProcessor
    )
    {
        this.logger = logger;
        this.replicationProcessingQueueProcessor = replicationProcessingQueueProcessor;
    }
    #endregion

    [Function(nameof(HReplicationProcessingQueueProcessorFunction))]
    public async Task Run(
        [ServiceBusTrigger("danh-hreplicationprocessingqueue", Connection = "AzureSBHReplicationPrimaryConnectionString")] ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions
    )
    {
        if (!(await replicationProcessingQueueProcessor.Process(message?.Body?.ToArray())).Ref(out var res))
        {
            logger.LogError(new OperationResultException(res), string.Join(", ", res.FlattenReasons()));
            await messageActions.DeadLetterMessageAsync(message, null, res.Reason, string.Join(",", [.. res.FlattenReasons() ?? [], .. res.Comments ?? []]).NullIfEmpty());
            return;
        }

        await messageActions.CompleteMessageAsync(message);
    }
}