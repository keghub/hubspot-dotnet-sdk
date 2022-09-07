using System;

namespace HubSpot.Converters {
    public class DateTimeOffSetConverter : ITypeConverter
    {
        public bool TryConvertTo(string value, out object result)
        {
            result = default;

            if (long.TryParse(value, out var epoch))
            {
                result = DateTimeOffset.FromUnixTimeMilliseconds(epoch);
                return true;
            }

            return false;
        }

        public bool TryConvertFrom(object value, out string result)
        {
            if (value is DateTimeOffset dto)
            {
                result = dto.ToUnixTimeMilliseconds().ToString("D");
                return true;
            }

            result = null;
            return false;
        }
    }
}