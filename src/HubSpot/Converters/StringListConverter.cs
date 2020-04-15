using System.Collections.Generic;
using System.Linq;

namespace HubSpot.Converters
{
    public class StringListConverter : ITypeConverter
    {
        public bool TryConvertTo(string value, out object result)
        {
            result = default;

            if (value == null || string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            var list = value.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            result = list;
            return true;
        }

        public bool TryConvertFrom(object value, out string result)
        {
            if (value == null)
            {
                result = null;
                return true;
            }

            if (value is IList<string> list)
            {
                result = string.Join(";", list.Where(s => !string.IsNullOrWhiteSpace(s)));
                return true;
            }

            result = null;
            return false;
        }
    }
}