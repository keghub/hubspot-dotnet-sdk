using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HubSpot.Model.Owners;
using Kralizek.Extensions.Http;

namespace HubSpot
{
    public partial class HttpHubSpotClient : IHubSpotOwnerClient
    {
        async Task<IReadOnlyList<Owner>> IHubSpotOwnerClient.GetManyAsync(string email)
        {
            var builder = new HttpQueryStringBuilder();

            if (email != null)
            {
                builder.Add("email", email);
            }

            var response = await SendAsync<IReadOnlyList<Owner>>(HttpMethod.Get, "/owners/v2/owners/", builder.BuildQuery()).ConfigureAwait(false);

            return response;
        }
    }
}