using H.Necessaire;
using H.Necessaire.Serialization;
using H.Replication.Contracts.DataContracts;
using System.Text;

namespace H.Replication.DataCopy.Processor
{
    public static class ParsingXtnx
    {
        public static OperationResult<HReplicationProcessingQueueEntry> TryParse(this byte[] body)
        {
            if (body.IsEmpty())
                return "Body is empty";

            if (!HSafe.Run(() => Encoding.UTF8.GetString(body)).Ref(out var decodeRes, out var json))
                return decodeRes.WithoutPayload<HReplicationProcessingQueueEntry>();

            if (!json.TryJsonToObject<HReplicationProcessingQueueEntry>().Ref(out var parseRes, out var result))
                return parseRes.WithoutPayload<HReplicationProcessingQueueEntry>();

            return result;
        }
    }
}
