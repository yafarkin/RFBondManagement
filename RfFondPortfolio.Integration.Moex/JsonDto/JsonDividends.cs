using Newtonsoft.Json;

namespace RfFondPortfolio.Integration.Moex.JsonDto
{
    internal class JsonDividends
    {
        [JsonProperty("dividends")]
        public JsonBase Dividends { get; set; }
    }
}