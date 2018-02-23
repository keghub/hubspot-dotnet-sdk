using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Model;

namespace HubSpot.Contacts.Filters {
    public class SearchContactFilter : ContactFilter
    {
        private readonly string _searchQuery;

        public SearchContactFilter(string searchQuery)
        {
            _searchQuery = searchQuery ?? throw new ArgumentNullException(nameof(searchQuery));
        }

        protected override async Task<(IReadOnlyList<Model.Contacts.Contact> list, bool hasMore, long? offset)> Get(IHubSpotClient client, IReadOnlyList<IProperty> propertiesToQuery, long? offset)
        {
            var response = await client.Contacts.SearchAsync(_searchQuery, propertiesToQuery, 25, offset);

            return (response.Contacts, response.HasMore, response.Offset);
        }
    }
}