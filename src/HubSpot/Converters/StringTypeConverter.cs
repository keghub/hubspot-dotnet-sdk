namespace HubSpot.Converters {
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
}