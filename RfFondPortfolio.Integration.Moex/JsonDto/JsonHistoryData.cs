using Newtonsoft.Json;

namespace RfBondManagement.Engine.Integration.Moex.Dto
{
    public class JsonHistoryData
    {
        [JsonProperty("history")]
        public JsonBase History { get; set; }

        [JsonProperty("history.cursor")]
        public JsonCursor Cursor { get; set; }
    }
}