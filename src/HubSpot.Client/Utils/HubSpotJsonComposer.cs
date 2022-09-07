using HubSpot.Model;
using HubSpot.Model.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HubSpot.Utils
{
    public class HubSpotJsonComposer
    {
        protected string _json;
        protected PostRequestModel _request;
        public HubSpotJsonComposer()
        {
            _request = new PostRequestModel();
        }

        public object GetJsonBodyObject()
        {
            return _request;
        }

        public void AddFilter(string propertyName, object value)
        {
            _request.Filters.Add(new FilterModel(propertyName, value));
        }

        public void AddArrayFilter<T>(string propertyName, IReadOnlyCollection<T> value, FilterOperator filterOperator)
        {
            if (filterOperator == FilterOperator.IN)
            {
                _request.Filters.Add(new FilterModel(propertyName, value.Cast<object>().ToList(), filterOperator.ToString()));
                return;
            }
        }

        public void AddProperties(IReadOnlyList<IProperty> properties)
        {
            if(properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }
            _request.RequestedProperties.AddRange(properties.Select(x => x.Name));
        }
    }
}
