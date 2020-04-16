using System.Linq;

namespace HubSpot.Converters
{
    public class StringArrayConverter : ITypeConverter
    {
        public bool TryConvertTo(string value, out object result)
        {
            result = default;

            if (value == null || string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            result = value.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray();
            return true;
        }

        public bool TryConvertFrom(object value, out string result)
        {
            if (value == null)
            {
                result = null;
                return true;
            }

            if (value is string[] array)
            {
                result = string.Join(";", array.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()));
                return true;
            }

            result = null;
            return false;
        }
    }
}