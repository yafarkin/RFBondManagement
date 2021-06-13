using Newtonsoft.Json;

namespace RfFondPortfolio.Integration.Moex.JsonDto
{
    internal class JsonHistoryData
    {
        [JsonProperty("history")]
        public JsonBase History { get; set; }

        [JsonProperty("history.cursor")]
        public JsonCursor Cursor { get; set; }
    }
}