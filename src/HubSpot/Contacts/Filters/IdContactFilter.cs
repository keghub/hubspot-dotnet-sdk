using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HubSpot.Model;
using HubSpot.Model.Contacts;

namespace HubSpot.Contacts.Filters {
    public class IdContactFilter : ContactFilter
    {
        private readonly IReadOnlyList<long> _contactIds;

        public IdContactFilter(IReadOnlyList<long> contactIds)
        {
            _contactIds = contactIds;
        }

        protected override async Task<(IReadOnlyList<Model.Contacts.Contact> list, bool hasMore, long? offset)> Get(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, long? offset)
        {
            var response = await client.Contacts.GetManyByIdAsync(_contactIds, propertiesToQuery, PropertyMode.ValueOnly, FormSubmissionMode.None, false, false).ConfigureAwait(false);

            return (response.Values.ToArray(), false, null);
        }
    }
}