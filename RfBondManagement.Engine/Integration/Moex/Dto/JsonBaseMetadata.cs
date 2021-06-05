using Newtonsoft.Json;

namespace RfBondManagement.Engine.Integration.Moex.Dto
{
    public class JsonBaseMetadata
    {
        public string Name { get; set; }

        public JsonBaseMetadataProperty Properties { get; set; }
    }

    public class JsonBaseMetadataProperty
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("bytes")]
        public int Bytes { get; set; }

        [JsonProperty("max_size")]
        public long MaxSize { get; set; }
    }
}