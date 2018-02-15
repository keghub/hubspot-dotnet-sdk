using System;

namespace HubSpot
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DefaultPropertyAttribute : Attribute
    {
        public string PropertyName { get; }

        public DefaultPropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}