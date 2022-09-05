using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HubSpot.Model.Requests
{
    public class FilterModel
    {
        public FilterModel()
        {

        }

        public FilterModel(string propertyName, object value)
        {        
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public FilterModel(string propertyName, List<object> values, string filterOperator)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            Values = values ?? throw new ArgumentNullException(nameof(values));
            Operator = filterOperator ?? throw new ArgumentNullException(nameof(filterOperator));
        }

        [JsonProperty("value")]
        public object Value { get; set; }

        [JsonProperty("values")]
        public List<object> Values { get; set; }

        [JsonProperty("propertyName")]
        public string PropertyName { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; } = FilterOperator.EQ.ToString();
    }

    public enum FilterOperator
    {
        EQ = 0,
        IN = 1
    }
}
