using Newtonsoft.Json;

namespace RfFondPortfolio.Integration.Moex.JsonDto
{
    internal class JsonPaperDefinition
    {
        [JsonProperty("description")]
        public JsonBase Description { get; set; }

        [JsonProperty("boards")]
        public JsonBase Boards { get; set; }
    }
}