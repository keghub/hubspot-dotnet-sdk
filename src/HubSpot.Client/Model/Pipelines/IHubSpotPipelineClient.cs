using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HubSpot.Model.Pipelines
{
    public interface IHubSpotPipelineClient
    {
        Task<Pipeline> GetByGuidAsync(string guid);
    }
}
