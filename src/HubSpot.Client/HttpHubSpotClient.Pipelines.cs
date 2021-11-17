using HubSpot.Model.Pipelines;
using Kralizek.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HubSpot
{
    public partial class HttpHubSpotClient : IHubSpotPipelineClient
    {
        async Task<Pipeline> IHubSpotPipelineClient.GetByGuidAsync(string guid)
        {
            try
            {
                var result = await _client.GetAsync<Pipeline>($"/deals/v1/pipelines/{guid}");
                return result;
            }
            catch (HttpException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException("Pipeline not found", ex);
            }
        }
    }
}
