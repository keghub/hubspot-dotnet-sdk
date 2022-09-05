using HubSpot.Model;
using HubSpot.Model.LineItems;
using HubSpot.Model.Pipelines;
using HubSpot.Model.Requests;
using HubSpot.Utils;
using Kralizek.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HubSpot
{
    public partial class HttpHubSpotClient : IHubSpotLineItemClient
    {
        async Task<LineItem> IHubSpotLineItemClient.GetAsync(long lineItemId, IReadOnlyList<IProperty> properties)
        {
            try
            {
                var builder = new HttpQueryStringBuilder();
                builder.AddProperties(properties, "properties");
                var result = await _client.GetAsync<LineItem>($"/crm/v3/objects/line_items/{lineItemId}", builder.BuildQuery());
                return result;
            }
            catch (HttpException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException("Line item not found", ex);
            }
        }

        async Task<LineItem> IHubSpotLineItemClient.GetBySKUAsync(IReadOnlyList<long> lineItemIds, IReadOnlyList<IProperty> properties, string sku)
        {
            try
            {
                var request = new HubSpotJsonComposer();
                request.AddFilter("hs_sku", sku);
                request.AddArrayFilter("hs_object_id", lineItemIds, FilterOperator.IN);
                request.AddProperties(properties);
                var newRequest = request.GetJsonBodyObject();
                var result = await _client.PostAsync<object, LineItemResult>($"/crm/v3/objects/line_items/search", request.GetJsonBodyObject());
                return result?.LineItems?.FirstOrDefault();
            }
            catch (HttpException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException("Educations line item could not be retrieved", ex);
            }
        }
    }
}
