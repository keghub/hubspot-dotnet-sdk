namespace HubSpot.Converters {
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
}