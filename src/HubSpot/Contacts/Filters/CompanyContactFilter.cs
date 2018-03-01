using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HubSpot.Internal;
using HubSpot.Model;
using HubSpot.Model.Companies;
using HubSpot.Model.Contacts;

namespace HubSpot.Contacts.Filters
{
    public class CompanyContactFilter : IContactFilter
    {
        private readonly long _companyId;

        public CompanyContactFilter(long companyId)
        {
            _companyId = companyId;
        }

        public int BatchSize { get; set; } = 15;

        public async Task<IReadOnlyList<Model.Contacts.Contact>> GetContacts(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery)
        {
            var contactIds = await GetAllContactIds();

            var contacts = new List<HubSpot.Model.Contacts.Contact>();

            foreach (var partition in contactIds.Batch(BatchSize))
            {
                var response = await client.Contacts.GetManyByIdAsync(partition.ToArray(), propertiesToQuery, PropertyMode.ValueOnly, FormSubmissionMode.None, false, false);

                contacts.AddRange(response.Values);
            }

            return contacts;

            async Task<IReadOnlyList<long>> GetAllContactIds()
            {
                var ids = new List<long>();

                ContactIdList list = null;

                do
                {
                    list = await client.Companies.GetContactIdsInCompanyAsync(_companyId, 25, list?.Offset);

                    ids.AddRange(list.ContactIds);

                } while (list.HasMore);

                return ids;
            }
        }
    }
}