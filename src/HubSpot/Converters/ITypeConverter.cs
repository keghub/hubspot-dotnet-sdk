using System;

namespace HubSpot.Converters
{
    public interface ITypeConverter
    {
        bool TryConvertTo(string value, out object result);

        bool TryConvertFrom(object value, out string result);
    }

    public class StringTypeConverter : ITypeConverter
    {
        public bool TryConvertTo(string value, out object result)
        {
            result = value;
            return true;
        }

        public bool TryConvertFrom(object value, out string result)
        {
            result = null;

            if (value == null)
            {
                return true;
            }

            if (value is string str)
            {
                result = str;
                return true;
            }

            return false;
        }
    }

    public class LongTypeConverter : ITypeConverter
    {
        public bool TryConvertTo(string value, out object result)
        {
            result = default;

            if (value == null)
            {
                return true;
            }

            if (long.TryParse(value, out var output))
            {
                result = output;
                return true;
            }

            return false;
        }

        public bool TryConvertFrom(object value, out string result)
        {
            if (value == null)
            {
                result = null;
                return true;
            }

            if (value is long num)
            {
                result = num.ToString("D");
                return true;
            }

            result = null;
            return false;
        }
    }

    public class DateTimeTypeConverter : ITypeConverter
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