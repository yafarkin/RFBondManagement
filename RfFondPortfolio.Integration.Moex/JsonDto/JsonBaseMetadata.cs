using Newtonsoft.Json;

namespace RfFondPortfolio.Integration.Moex.JsonDto
{
    internal class JsonBaseMetadata
    {
        public string Name { get; set; }

        public JsonBaseMetadataProperty Properties { get; set; }
    }

    internal class JsonBaseMetadataProperty
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("bytes")]
        public int Bytes { get; set; }

        [JsonProperty("max_size")]
        public long MaxSize { get; set; }
    }
}