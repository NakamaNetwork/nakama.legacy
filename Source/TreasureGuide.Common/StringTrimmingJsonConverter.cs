using System;
using Newtonsoft.Json;

namespace TreasureGuide.Common
{
    public class StringTrimmingJsonConverter : JsonConverter
    {
        public override bool CanWrite { get; } = false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var stringValue = reader.Value as string;
            return !String.IsNullOrWhiteSpace(stringValue) ? stringValue.Trim() : stringValue;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
