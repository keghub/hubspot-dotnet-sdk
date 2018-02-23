using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Model;
using HubSpot.Model.Contacts;

namespace HubSpot.Contacts.Filters {
    public class RecentlyUpdatedContactFilter : ContactFilter
    {
        protected override async Task<(IReadOnlyList<Model.Contacts.Contact> list, bool hasMore, long? offset)> Get(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, long? offset)
        {
            var response = await client.Contacts.GetRecentlyUpdatedAsync(propertiesToQuery, PropertyMode.ValueOnly, FormSubmissionMode.None, false, 25, offset).ConfigureAwait(false);

            return (response.Contacts, response.HasMore, response.ContactOffset);
        }
    }
}