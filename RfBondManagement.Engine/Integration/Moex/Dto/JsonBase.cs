using System.Collections.Generic;
using Newtonsoft.Json;

namespace RfBondManagement.Engine.Integration.Moex.Dto
{
    [JsonConverter(typeof(JsonBaseConverter))]
    public class JsonBase
    {
        [JsonProperty("metadata")]
        public List<JsonBaseMetadata> Metadata { get; set; }

        [JsonProperty("columns")]
        public List<string> Columns { get; set; }

        [JsonProperty("data")]
        public List<Dictionary<string, string>> Data { get; set; }
    }
}