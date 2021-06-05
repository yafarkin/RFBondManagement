using System.Collections.Generic;
using Newtonsoft.Json;

namespace RfBondManagement.Engine.Integration.Moex.Dto
{
    [JsonConverter(typeof(JsonBaseConverter))]
    public class JsonBase
    {
        [JsonProperty("metadata")]
        public List<JsonBaseMetadata> Metadata { get; set; }

        [JsonProperty("columns")]
        public List<string> Columns { get; set; }

        [JsonProperty("data")]
        public List<Dictionary<string, string>> Data { get; set; }

        public Dictionary<string, string> GetDataFor(string key, string value)
        {
            foreach (var dataItem in Data)
            {
                if (dataItem.ContainsKey(key) && dataItem[key] == value)
                {
                    return dataItem;
                }
            }

            return null;
        }

        public string GetDataFor(string key)
        {
            foreach (var dataItem in Data)
            {
                if (dataItem.ContainsKey(key))
                {
                    return dataItem[key];
                }
            }

            return null;
        }
    }
}