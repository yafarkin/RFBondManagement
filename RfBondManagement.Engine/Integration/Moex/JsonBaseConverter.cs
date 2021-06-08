using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RfBondManagement.Engine.Integration.Moex.Dto;

namespace RfBondManagement.Engine.Integration.Moex
{
    public class JsonBaseConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            reader.Read();

            JToken metadataToken = null;
            JToken columnsToken = null;
            JToken dataToken = null;

            while (null == metadataToken || null == columnsToken || null == dataToken)
            {
                var v = reader.Value + string.Empty;
                if (string.IsNullOrWhiteSpace(v))
                {
                    break;
                }

                var token = JToken.Load(reader);

                if (v == "columns")
                {
                    columnsToken = token;
                }
                else if (v == "data")
                {
                    dataToken = token;
                }
                else if (v == "metadata")
                {
                    metadataToken = token;
                }
            }

            //var metadataToken = JToken.Load(reader);
            //var columnsToken = JToken.Load(reader);
            //var dataToken = JToken.Load(reader);

            var result = Activator.CreateInstance(objectType) as JsonBase;

            result.Metadata = new List<JsonBaseMetadata>();
            result.Columns = new List<string>();
            result.Data = new List<Dictionary<string, string>>();

            if (metadataToken != null)
            {
                var metadatas = JObject.Load(metadataToken.First.CreateReader());
                foreach (var metaItem in metadatas)
                {
                    var metaData = new JsonBaseMetadata
                    {
                        Name = metaItem.Key,
                        Properties = metaItem.Value?.ToObject<JsonBaseMetadataProperty>()
                    };

                    result.Metadata.Add(metaData);
                }
            }

            if (columnsToken != null)
            {
                var columns = JArray.Load(columnsToken.First.CreateReader());
                foreach (var column in columns)
                {
                    result.Columns.Add(column.Value<string>());
                }
            }

            if (dataToken != null)
            {
                var datas = JArray.Load(dataToken.First.CreateReader());
                foreach (var dataItem in datas)
                {
                    var dataLineItems = JArray.Load(dataItem.CreateReader());
                    var dataResultItem = new Dictionary<string, string>();

                    for (var i = 0; i < dataLineItems.Count; i++)
                    {
                        dataResultItem.Add(result.Columns[i], dataLineItems[i].Value<string>());
                    }

                    result.Data.Add(dataResultItem);
                }
            }

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}