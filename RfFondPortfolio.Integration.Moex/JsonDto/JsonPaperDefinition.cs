using Newtonsoft.Json;

namespace RfBondManagement.Engine.Integration.Moex.Dto
{
    public class JsonPaperDefinition
    {
        [JsonProperty("description")]
        public JsonBase Description { get; set; }

        [JsonProperty("boards")]
        public JsonBase Boards { get; set; }
    }
}