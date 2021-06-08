using Newtonsoft.Json;

namespace RfBondManagement.Engine.Integration.Moex.Dto
{
    public class JsonLastPrice
    {
        [JsonProperty("securities")]
        public JsonBase Securities { get; set; }

        [JsonProperty("marketdata")]
        public JsonBase MarketData { get; set; }
    }
}