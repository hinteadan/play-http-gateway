using System.Collections.Generic;

namespace H.HttpGate.Contracts.Public.DataContracts
{
    public class HHttpGateRequest
    {
        public string HttpVersion { get; set; }
        public string HttpMethod { get; set; }
        public string URL { get; set; }
        public Dictionary<string, string[]> Headers { get; set; }
        public string Content { get; set; }
    }
}
