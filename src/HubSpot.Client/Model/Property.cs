using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSpot.Model
{
    public interface IProperty
    {
        string Name { get; }
    }

    public interface IUpdateableProperty : IProperty
    {
        ValuedProperty Value(string value);
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

    public class PropertyList<TProperty> where TProperty : IValuedProperty
    {
        [JsonProperty("properties")]
        public IReadOnlyList<TProperty> Properties { get; set; }
    }

    public class ObjectPropertyList<TProperty> : PropertyList<TProperty> where TProperty : IValuedProperty
    {
        [JsonProperty("objectId")]
        public long ObjectId { get; set; }
    }

    public interface IValuedProperty
    {
        string Property { get; }

        string Value { get; }
    }

    public class ValuedProperty : IValuedProperty
    {
        public ValuedProperty(string propertyName, string value)
        {
            Property = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            Value = value;
        }

        [JsonProperty("property")]
        public string Property { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class ValuedPropertyV2 : IValuedProperty
    {
        public ValuedPropertyV2(string propertyName, string value)
        {
            Property = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            Value = value;
        }

        [JsonProperty("name")]
        public string Property { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class PagedList<T>
    {
        [JsonProperty("results")]
        public IReadOnlyList<T> Results { get; set; }

        [JsonProperty("hasMore")]
        public bool HasMore { get; set; }

        [JsonProperty("offset")]
        public long? Offset { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }
}