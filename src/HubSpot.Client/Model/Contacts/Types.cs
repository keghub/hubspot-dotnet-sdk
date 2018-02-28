using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSpot.Model.Contacts
{
    public class CreateOrUpdateResponse
    {
        [JsonProperty("vid")]
        public long Id { get; set; }

        [JsonProperty("isNew")]
        public bool IsNew { get; set; }
    }

    public class DeleteContactResponse
    {
        [JsonProperty("vid")]
        public long Id { get; set; }

        [JsonProperty("deleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    public class SearchResponse
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("offset")]
        public long? Offset { get; set; }

        [JsonProperty("has-more")]
        public bool HasMore { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("contacts")]
        public IReadOnlyList<Contact> Contacts { get; set; }
    }

    public class ContactList
    {
        [JsonProperty("contacts")]
        public IReadOnlyList<ContactListItem> Contacts { get; set; }

        [JsonProperty("has-more")]
        public bool HasMore { get; set; }

        [JsonProperty("vid-offset")]
        public long? ContactOffset { get; set; }

        [JsonProperty("time-offset")]
        public DateTimeOffset? TimeOffset { get; set; }
    }

    public class ContactListItem : Contact
    {
        [JsonProperty("addedAt")]
        public DateTimeOffset AddedAt { get; set; }
    }
}