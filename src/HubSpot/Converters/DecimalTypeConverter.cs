using System.Globalization;

namespace HubSpot.Converters {
    public class DecimalTypeConverter : ITypeConverter
    {
        public bool TryConvertTo(string value, out object result)
        {
            result = default;

            if (value == null)
            {
                return true;
            }

            if (decimal.TryParse(value, out var output))
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

            if (value is decimal num)
            {
                result = num.ToString("G", CultureInfo.InvariantCulture);
                return true;
            }

            result = null;
            return false;
        }
    }
}