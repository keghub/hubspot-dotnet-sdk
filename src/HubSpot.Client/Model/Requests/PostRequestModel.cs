using HubSpot.Model.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HubSpot.Model
{
    public class PostRequestModel
    {
        [JsonProperty("filters")]
        public List<FilterModel> Filters { get; set; } = new List<FilterModel>();

        [JsonProperty("properties")]
        public List<string> RequestedProperties { get; set; } = new List<string>();
    }
}
