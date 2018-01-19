using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSpot.Model.Contacts
{
    public class Contact
    {
        [JsonProperty("vid")]
        public long Id { get; set; }

        [JsonProperty("canonical-vid")]
        public long CanonicalId { get; set; }

        [JsonProperty("merged-vids")]
        public long[] MergedIds { get; set; }

        [JsonProperty("portal-id")]
        public long PortalId { get; set; }

        [JsonProperty("is-contact")]
        public bool IsContact { get; set; }

        [JsonProperty("profile-token")]
        public string ProfileToken { get; set; }

        [JsonProperty("profile-url")]
        public Uri ProfileUrl { get; set; }

        [JsonProperty("properties")]
        public IReadOnlyDictionary<string, PropertyValue> Properties { get; set; }

        [JsonProperty("form-submissions")]
        public IReadOnlyList<FormSubmission> FormSubmissions { get; set; }

        [JsonProperty("list-memberships")]
        public IReadOnlyList<ListMembership> ListMemberships { get; set; }

        [JsonProperty("identity-profiles")]
        public IReadOnlyList<IdentityProfile> IdentityProfiles { get; set; }
    }

    public class PropertyValue
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("versions")]
        public IReadOnlyList<TimestampedValue> Versions { get; set; } = Array.Empty<TimestampedValue>();
    }

    public class TimestampedValue
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("source-type")]
        public string SourceType { get; set; }

        [JsonProperty("source-id")]
        public string SourceId { get; set; }

        [JsonProperty("source-label")]
        public string SourceLabel { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }
    }

    public class FormSubmission
    {
        [JsonProperty("conversion-id")]
        public string ConversionId { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("form-id")]
        public string FormId { get; set; }

        [JsonProperty("page-url")]
        public Uri PageUrl { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class ListMembership
    {
        [JsonProperty("static-list-id")]
        public int ListId { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("vid")]
        public int Id { get; set; }

        [JsonProperty("is-member")]
        public bool IsMember { get; set; }
    }

    public class IdentityProfile
    {
        [JsonProperty("vid")]
        public long Id { get; set; }

        [JsonProperty("saved-at-timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("identities")]
        public IReadOnlyList<Identity> Identities { get; set; }

    }

    public class Identity
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }
    }
}
