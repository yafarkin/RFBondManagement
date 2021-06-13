using Newtonsoft.Json;

namespace RfFondPortfolio.Integration.Moex.JsonDto
{
    internal class JsonLastPrice
    {
        [JsonProperty("securities")]
        public JsonBase Securities { get; set; }

        [JsonProperty("marketdata")]
        public JsonBase MarketData { get; set; }
    }
}