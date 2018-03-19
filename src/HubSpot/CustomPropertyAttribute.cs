using System;
using System.Collections.Generic;
using System.Text;

namespace HubSpot
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CustomPropertyAttribute : Attribute
    {
        public string FieldName { get; }

        public bool IsReadOnly { get; set; }

        public CustomPropertyAttribute(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}