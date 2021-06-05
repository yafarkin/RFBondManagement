using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RfBondManagement.Engine.Integration.Moex.Dto;

namespace RfBondManagement.Engine.Integration.Moex
{
    public class JsonBaseConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            reader.Read();
            var metadataToken = JToken.Load(reader);
            var columnsToken = JToken.Load(reader);
            var dataToken = JToken.Load(reader);

            var result = new JsonBase
            {
                Metadata = new List<JsonBaseMetadata>(),
                Columns = new List<string>(),
                Data = new List<Dictionary<string, string>>()
            };

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

            var columns = JArray.Load(columnsToken.First.CreateReader());
            foreach (var column in columns)
            {
                result.Columns.Add(column.Value<string>());
            }

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

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}