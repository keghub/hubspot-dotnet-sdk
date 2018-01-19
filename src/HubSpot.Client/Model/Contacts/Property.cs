using System;
using Newtonsoft.Json;

namespace HubSpot.Model.Contacts
{
    public interface IProperty
    {
        string Name { get; }
    }

    public interface IUpdateableProperty : IProperty
    {
        ValuedProperty Value(string value);
    }

    public static class Properties
    {
        public static readonly IProperty LastModifiedDate = new Property("lastmodifieddate");
        public static readonly IProperty AssociatedCompanyId = new Property("associatedcompanyid");
        public static readonly IProperty CreateDate = new Property("createdate");

        public static readonly IUpdateableProperty FirstName = new Property("firstname");
        public static readonly IUpdateableProperty LastName = new Property("lastname");
        public static readonly IUpdateableProperty Email = new Property("email");
    }


    public class Property : IUpdateableProperty
    {
        public Property(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public ValuedProperty Value(string value) => new ValuedProperty(Name, value);
    }


    public class ValuedProperty
    {
        public ValuedProperty(string propertyName, string value)
        {
            Property = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        [JsonProperty("property")]
        public string Property { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}