using System;
using System.Collections.Generic;
using System.Text;

namespace HubSpot.Converters
{
    public class DateTimeConverter : ITypeConverter
    {
        public bool TryConvertTo(string value, out object result)
        {
            result = default;

            if (DateTime.TryParse(value, out var dateTime))
            {
                result = dateTime;
                return true;
            }

            return false;
        }

        public bool TryConvertFrom(object value, out string result)
        {
            if (value is DateTime dt)
            {
                result = dt.ToString("G");
                return true;
            }

            result = null;
            return false;
        }
    }
}
