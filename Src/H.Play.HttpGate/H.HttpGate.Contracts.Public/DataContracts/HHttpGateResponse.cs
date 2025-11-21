using System.Collections.Generic;

namespace H.HttpGate.Contracts.Public.DataContracts
{
    public class HHttpGateResponse
    {
        public HHttpGateRequest Request { get; set; }
        public int HttpStatusCode { get; set; }
        public string HttpStatusLabel { get; set; }
        public string HttpStatusReason { get; set; }
        public Dictionary<string, string[]> Headers { get; set; }
        public string Content { get; set; }
    }
}
