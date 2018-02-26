using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSpot.Model.Companies
{
    public class Contact
    {
        [JsonProperty("vid")]
        public long Id { get; set; }

        [JsonProperty("portalid")]
        public long PortalId { get; set; }

        [JsonProperty("isContact")]
        public bool IsContact { get; set; }

        [JsonProperty("publicToken")]
        public string PublicToken { get; set; }

        [JsonProperty("canonicalVid")]
        public long CanonicalVid { get; set; }

        [JsonProperty("properties")]
        public IReadOnlyList<ContactProperty> Properties { get; set; }
    }

    public class ContactProperty
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class ContactList
    {
        [JsonProperty("contacts")]
        public IReadOnlyList<Contact> Contacts { get; set; }

        [JsonProperty("vidOffset")]
        public long? Offset { get; set; }

        [JsonProperty("hasMore")]
        public bool HasMore { get; set; }
    }

    public class ContactIdList
    {
        [JsonProperty("vids")]
        public IReadOnlyList<long> ContactIds { get; set; }

        [JsonProperty("vidOffset")]
        public long? Offset { get; set; }

        [JsonProperty("hasMore")]
        public bool HasMore { get; set; }

        public void Deconstruct(out IReadOnlyList<long> items, out bool hasMore, out long? offset)
        {
            items = ContactIds;
            hasMore = HasMore;
            offset = Offset;
        }
    }
}