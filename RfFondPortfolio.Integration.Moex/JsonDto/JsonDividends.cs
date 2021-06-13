using Newtonsoft.Json;

namespace RfBondManagement.Engine.Integration.Moex.Dto
{
    public class JsonDividends
    {
        [JsonProperty("dividends")]
        public JsonBase Dividends { get; set; }
    }
}