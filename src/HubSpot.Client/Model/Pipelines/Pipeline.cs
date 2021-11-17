using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HubSpot.Model.Pipelines
{
    public class Pipeline
    {
        [JsonProperty("active")]
        public bool IsActive { get; set; }

        [JsonProperty("displayOrder")]
        public int DisplayOrder { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("pipelineid")]
        public string Guid { get; set; }

        [JsonProperty("stages")]
        public IReadOnlyList<StageProperty> Stages { get; set; }
    }

    public class StageProperty
    { 
        [JsonProperty("active")]
        public bool IsActive { get; set; }

        [JsonProperty("closedWon")]
        public bool ClosedWon { get; set; }

        [JsonProperty("displayOrder")]
        public int DisplayOrder { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("probability")]
        public decimal Probability { get; set; }

        [JsonProperty("stageId")]
        public string StageID { get; set; }
    }
}
