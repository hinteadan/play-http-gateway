using H.Necessaire;
using H.Replication.Contracts;

namespace H.Replication.DataCopy.Processor
{
    public static class ProcessingXtnx
    {
        public static async Task<OperationResult> Process(this ImAnHReplicationProcessingQueueProcessor replicationProcessingQueueProcessor, byte[] messageBody)
        {
            if (!messageBody.TryParse().Ref(out var parseRes, out var processingQueueEntry))
                return parseRes;

            if (!(await replicationProcessingQueueProcessor.Process(processingQueueEntry)).Ref(out var processingRes))
                return processingRes;

            return true;
        }
    }
}
