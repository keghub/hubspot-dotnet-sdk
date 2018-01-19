using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HubSpot.Utils
{
    public class UnixEpochConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value)
            {
                case DateTimeOffset dto:
                    writer.WriteValue(dto.ToUnixTimeSeconds());
                    break;

                case DateTime dt:
                    var converted = new DateTimeOffset(dt.ToUniversalTime(), TimeSpan.Zero);
                    writer.WriteValue(converted.ToUnixTimeSeconds());
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = (long)reader.Value;

            if (objectType == typeof(DateTimeOffset))
                return DateTimeOffset.FromUnixTimeMilliseconds(value);

            if (objectType == typeof(DateTime))
                return DateTimeOffset.FromUnixTimeMilliseconds(value).UtcDateTime;

            return null;
        }
    }
}