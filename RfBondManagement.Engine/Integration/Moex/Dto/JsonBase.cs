using System;
using System.Collections.Generic;
using System.Globalization;
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

        public string GetDataForString(string key, string value, string field)
        {
            var l = GetDataFor(key, value);
            if (null == l || !l.ContainsKey(field))
            {
                return null;
            }

            return l[field];
        }

        public DateTime? GetDataForDateTime(string key, string value, string field)
        {
            var s = GetDataForString(key, value, field);
            if (null == s)
            {
                return null;
            }

            return DateTime.TryParse(s, out var r) ? (DateTime?) r : null;
        }

        public decimal? GetDataForDecimal(string key, string value, string field)
        {
            var s = GetDataForString(key, value, field);
            if (null == s)
            {
                return null;
            }

            return decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var r) ? (decimal?)r : null;
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