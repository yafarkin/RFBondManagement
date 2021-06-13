using Newtonsoft.Json;

namespace RfFondPortfolio.Integration.Moex.JsonDto
{
    internal class JsonBondization
    {
        [JsonProperty("amortizations")]
        public JsonBase Amortizations { get; set; }

        [JsonProperty("coupons")]
        public JsonBase Coupons { get; set; }

        [JsonProperty("offers")]
        public JsonBase Offers { get; set; }
    }
}