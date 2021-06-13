using Newtonsoft.Json;

namespace RfBondManagement.Engine.Integration.Moex.Dto
{
    public class JsonBondization
    {
        [JsonProperty("amortizations")]
        public JsonBase Amortizations { get; set; }

        [JsonProperty("coupons")]
        public JsonBase Coupons { get; set; }

        [JsonProperty("offers")]
        public JsonBase Offers { get; set; }
    }
}