using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Model;

namespace HubSpot.Contacts.Filters
{
    public abstract class ContactFilter : IContactFilter
    {
        public async Task<IReadOnlyList<Model.Contacts.Contact>> GetContacts(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery)
        {
            long? offset = null;

            var result = new List<Model.Contacts.Contact>();

            (IReadOnlyList<Model.Contacts.Contact> list, bool hasMore, long? offset) tuple;

            do
            {
                tuple = await Get(client, propertiesToQuery, offset).ConfigureAwait(false);

                offset = tuple.offset;

                result.AddRange(tuple.list);

            } while (tuple.hasMore);

            return result;
        }

        protected abstract Task<(IReadOnlyList<Model.Contacts.Contact> list, bool hasMore, long? offset)> Get(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, long? offset);
    }
}
