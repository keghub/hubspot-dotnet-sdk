using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSpot.Model.Contacts
{
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