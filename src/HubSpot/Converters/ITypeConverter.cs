using System;

namespace HubSpot.Converters
{
    public interface ITypeConverter
    {
        object Convert(string value);
    }

    public class StringTypeConverter : ITypeConverter
    {
        public object Convert(string value) => value;
    }
}