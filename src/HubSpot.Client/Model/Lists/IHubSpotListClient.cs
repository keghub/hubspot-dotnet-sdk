using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Model.Contacts;
using Newtonsoft.Json;

namespace HubSpot.Model.Lists
{
    public interface IHubSpotListClient
    {
        Task<List> CreateAsync(string name, bool dynamic = false, IReadOnlyList<IReadOnlyList<Filter>> filters = null);

        Task<ListList> GetAllAsync(int count = 20, long? offset = null);

        Task<List> GetByIdAsync(long listId);

        Task<List> UpdateAsync(long listId, string name = null, bool? dynamic = null, IReadOnlyList<IReadOnlyList<Filter>> filters = null);

        Task DeleteAsync(long listId);

        Task<ListList> GetManyByIdAsync(IReadOnlyList<long> listIds);

        Task<ListList> GetAllStaticAsync(int count = 20, long? offset = null);

        Task<ListList> GetAllDynamicAsync(int count = 20, long? offset = null);

        Task<ContactList> GetContactsInListAsync(long listId, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, int count = 20, long? contactOffset = null);

        Task<ContactList> GetContactsRecentlyAddedToListAsync(long listId, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, int count = 20, long? contactOffset = null, DateTimeOffset? timeOffset = null);

        Task<ContactListResponse> AddContactsToListAsync(long listId, IReadOnlyList<long> contactIds = null, IReadOnlyList<string> contactEmails = null);

        Task<ContactListResponse> RemoveContactFromListAsync(long listId, long contactId);
    }

    public class ListList
    {
        [JsonProperty("lists")]
        public IReadOnlyList<List> Lists { get; set; }

        [JsonProperty("has-more")]
        public bool HasMore { get; set; }

        [JsonProperty("offset")]
        public long? Offset { get; set; }

        public static readonly ListList Empty = new ListList
        {
            HasMore = false,
            Lists = new List[0],
            Offset = null
        };
    }

    public class ContactListResponse
    {
        [JsonProperty("updated")]
        public IReadOnlyList<long> Updated { get; set; }

        [JsonProperty("discarded")]
        public IReadOnlyList<long> Discarded { get; set; }

        [JsonProperty("invalidVids")]
        public IReadOnlyList<long> InvalidIds { get; set; }

        [JsonProperty("invalidEmails")]
        public IReadOnlyList<long> InvalidEmails { get; set; }
    }
}