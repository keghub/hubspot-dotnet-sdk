using System;

namespace HubSpot.Converters
{
    public interface ITypeConverter
    {
        bool TryConvertTo(string value, out object result);

        bool TryConvertFrom(object value, out string result);
    }
}