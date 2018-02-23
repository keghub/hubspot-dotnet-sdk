using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HubSpot.Model;
using HubSpot.Model.Contacts;

namespace HubSpot.Contacts.Filters {
    public class EmailContactFilter : ContactFilter
    {
        private readonly IReadOnlyList<string> _emails;

        public EmailContactFilter(IReadOnlyList<string> emails)
        {
            _emails = emails;
        }

        protected override async Task<(IReadOnlyList<Model.Contacts.Contact> list, bool hasMore, long? offset)> Get(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, long? offset)
        {
            var response = await client.Contacts.GetManyByEmailAsync(_emails, propertiesToQuery, PropertyMode.ValueOnly, FormSubmissionMode.None, false, false).ConfigureAwait(false);

            return (response.Values.ToArray(), false, null);
        }
    }
}