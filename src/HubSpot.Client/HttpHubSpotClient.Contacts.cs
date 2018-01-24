using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using HubSpot.Utils;

namespace HubSpot
{
    public static class HttpQueryStringBuilderContactExtensions
    {
        public static void AddProperties(this HttpQueryStringBuilder builder, IEnumerable<IProperty> properties, string fieldName = "property")
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            foreach (var property in properties ?? Array.Empty<IProperty>())
            {
                builder.Add(fieldName, property.Name);
            }
        }

        public static void AddShowListMemberships(this HttpQueryStringBuilder builder, bool showListMemberships)
        {
            builder.Add("showListMemberships", showListMemberships ? "true" : "false");
        }

        public static void AddFormSubmissionMode(this HttpQueryStringBuilder builder, FormSubmissionMode formSubmissionMode)
        {
            builder.Add("formSubmissionMode", GetFormSubmissionMode(formSubmissionMode));

            string GetFormSubmissionMode(FormSubmissionMode mode)
            {
                switch (mode)
                {
                    case FormSubmissionMode.All: return "all";
                    case FormSubmissionMode.Newest: return "newest";
                    case FormSubmissionMode.None: return "none";
                    case FormSubmissionMode.Oldest: return "oldest";

                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode));
                }
            }
        }

        public static void AddPropertyMode(this HttpQueryStringBuilder builder, PropertyMode propertyMode)
        {
            builder.Add("propertyMode", propertyMode == PropertyMode.ValueAndHistory ? "value_and_history" : "value_only");
        }
    }

    public partial class HttpHubSpotClient : IHubSpotContactClient
    {
        async Task<Contact> IHubSpotContactClient.GetByIdAsync(long contactId, IReadOnlyList<IProperty> properties, PropertyMode propertyMode = PropertyMode.ValueAndHistory, FormSubmissionMode formSubmissionMode = FormSubmissionMode.All, bool showListMemberships = true)
        {
            var builder = new HttpQueryStringBuilder();

            builder.AddProperties(properties);
            builder.AddPropertyMode(propertyMode);
            builder.AddFormSubmissionMode(formSubmissionMode);
            builder.AddShowListMemberships(showListMemberships);

            var contact = await SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/vid/{contactId}/profile", builder.BuildQuery());

            return contact;
        }

        async Task<Contact> IHubSpotContactClient.GetByEmailAsync(string email, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueAndHistory, FormSubmissionMode formSubmissionMode = FormSubmissionMode.All, bool showListMemberships = true)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var builder = new HttpQueryStringBuilder();

            builder.AddProperties(properties);
            builder.AddPropertyMode(propertyMode);
            builder.AddFormSubmissionMode(formSubmissionMode);
            builder.AddShowListMemberships(showListMemberships);

            var contact = await SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/email/{email}/profile", builder.BuildQuery());

            return contact;
        }

        async Task<Contact> IHubSpotContactClient.GetByUserTokenAsync(string userToken, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueAndHistory, FormSubmissionMode formSubmissionMode = FormSubmissionMode.All, bool showListMemberships = true)
        {
            if (string.IsNullOrEmpty(userToken))
            {
                throw new ArgumentNullException(nameof(userToken));
            }

            var builder = new HttpQueryStringBuilder();

            builder.AddProperties(properties);
            builder.AddPropertyMode(propertyMode);
            builder.AddFormSubmissionMode(formSubmissionMode);
            builder.AddShowListMemberships(showListMemberships);

            var contact = await SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/utk/{userToken}/profile", builder.BuildQuery());

            return contact;
        }

        async Task<IReadOnlyDictionary<long, Contact>> IHubSpotContactClient.GetManyByIdAsync(IReadOnlyList<long> contactIds, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, bool includeDeletes = false)
        {
            if (contactIds == null || contactIds.Count == 0)
            {
                return new Dictionary<long, Contact>();
            }

            if (contactIds.Count >= 100)
            {
                throw new ArgumentOutOfRangeException(nameof(contactIds), "Up to 100 contacts can be requested at the same time");
            }

            var builder = new HttpQueryStringBuilder();

            foreach (var id in contactIds)
            {
                builder.Add("vid", id.ToString());
            }

            builder.AddProperties(properties);
            builder.AddPropertyMode(propertyMode);
            builder.AddFormSubmissionMode(formSubmissionMode);
            builder.AddShowListMemberships(showListMemberships);

            var contacts = await SendAsync<Dictionary<long, Contact>>(HttpMethod.Get, "/contacts/v1/contact/vids/batch/", builder.BuildQuery());

            return contacts;
        }

        async Task<IReadOnlyDictionary<long, Contact>> IHubSpotContactClient.GetManyByEmailAsync(IReadOnlyList<string> emails, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, bool includeDeletes = false)
        {
            if (emails == null || emails.Count == 0)
            {
                return new Dictionary<long, Contact>();
            }

            if (emails.Count >= 100)
            {
                throw new ArgumentOutOfRangeException(nameof(emails), "Up to 100 contacts can be requested at the same time");
            }

            var builder = new HttpQueryStringBuilder();

            foreach (var email in emails)
            {
                builder.Add("email", email);
            }

            builder.AddProperties(properties);
            builder.AddPropertyMode(propertyMode);
            builder.AddFormSubmissionMode(formSubmissionMode);
            builder.AddShowListMemberships(showListMemberships);

            var contacts = await SendAsync<Dictionary<long, Contact>>(HttpMethod.Get, "/contacts/v1/contact/emails/batch/", builder.BuildQuery());

            return contacts;

        }

        async Task<ContactList> IHubSpotContactClient.GetAllAsync(IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, int count = 20, long? contactOffset = null)
        {
            if (count > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Up to 100 contacts can be requested at the same time");
            }

            var builder = new HttpQueryStringBuilder();

            builder.AddProperties(properties);
            builder.AddPropertyMode(propertyMode);
            builder.AddFormSubmissionMode(formSubmissionMode);
            builder.AddShowListMemberships(showListMemberships);
            builder.Add("count", count.ToString());

            if (contactOffset.HasValue)
            {
                builder.Add("vidOffset", contactOffset.Value.ToString());
            }

            var list = await SendAsync<ContactList>(HttpMethod.Get, "/contacts/v1/lists/all/contacts/all", builder.BuildQuery());

            return list;
        }

        async Task<ContactList> IHubSpotContactClient.GetRecentlyUpdatedAsync(IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, int count = 20, long? contactOffset = null, DateTimeOffset? timeOffset = null)
        {
            if (count > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Up to 100 contacts can be requested at the same time");
            }

            var builder = new HttpQueryStringBuilder();

            builder.AddProperties(properties);
            builder.AddPropertyMode(propertyMode);
            builder.AddFormSubmissionMode(formSubmissionMode);
            builder.AddShowListMemberships(showListMemberships);
            builder.Add("count", count.ToString());

            if (contactOffset.HasValue)
            {
                builder.Add("vidOffset", contactOffset.Value.ToString());
            }

            if (timeOffset.HasValue)
            {
                builder.Add("timeOffset", timeOffset.Value.ToUnixTimeMilliseconds().ToString());
            }

            var list = await SendAsync<ContactList>(HttpMethod.Get, "/contacts/v1/lists/recently_updated/contacts/recent", builder.BuildQuery());

            return list;

        }

        async Task<ContactList> IHubSpotContactClient.GetRecentlyCreatedAsync(IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, int count = 20, long? contactOffset = null, DateTimeOffset? timeOffset = null)
        {
            if (count > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Up to 100 contacts can be requested at the same time");
            }

            var builder = new HttpQueryStringBuilder();

            builder.AddProperties(properties);
            builder.AddPropertyMode(propertyMode);
            builder.AddFormSubmissionMode(formSubmissionMode);
            builder.AddShowListMemberships(showListMemberships);
            builder.Add("count", count.ToString());

            if (contactOffset.HasValue)
            {
                builder.Add("vidOffset", contactOffset.Value.ToString());
            }

            if (timeOffset.HasValue)
            {
                builder.Add("timeOffset", timeOffset.Value.ToUnixTimeMilliseconds().ToString());
            }

            var list = await SendAsync<ContactList>(HttpMethod.Get, "/contacts/v1/lists/all/contacts/recent", builder.BuildQuery());

            return list;
        }

        async Task<DeleteContactResponse> IHubSpotContactClient.DeleteAsync(long contactId)
        {
            var response = await SendAsync<DeleteContactResponse>(HttpMethod.Delete, $"/contacts/v1/contact/vid/{contactId}");
            return response;
        }

        async Task<Contact> IHubSpotContactClient.CreateAsync(IReadOnlyList<ValuedProperty> properties)
        {
            var propertyList = new PropertyList
            {
                Properties = properties
            };

            var contact = await SendAsync<PropertyList, Contact>(HttpMethod.Post, propertyList, "/contacts/v1/contact");

            return contact;
        }

        async Task IHubSpotContactClient.UpdateByIdAsync(long contactId, IReadOnlyList<ValuedProperty> properties)
        {
            var propertyList = new PropertyList
            {
                Properties = properties
            };

            await SendAsync(HttpMethod.Post, propertyList, $"/contacts/v1/contact/vid/{contactId}/profile");
        }

        async Task IHubSpotContactClient.UpdateByEmailAsync(string email, IReadOnlyList<ValuedProperty> properties)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var propertyList = new PropertyList
            {
                Properties = properties
            };

            await SendAsync(HttpMethod.Post, propertyList, $"/contacts/v1/contact/email/{email}/profile");
        }

        async Task<CreateOrUpdateResponse> IHubSpotContactClient.CreateOrUpdateByEmailAsync(string email, IReadOnlyList<ValuedProperty> properties)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var propertyList = new PropertyList
            {
                Properties = properties
            };

            var response = await SendAsync<PropertyList, CreateOrUpdateResponse>(HttpMethod.Post, propertyList, $"/contacts/v1/contact/createOrUpdate/email/{email}");

            return response;
        }

        async Task<SearchResponse> IHubSpotContactClient.SearchAsync(string query, IReadOnlyList<IProperty> properties = null, int count = 20, long? contactOffset = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (count > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Up to 100 contacts can be requested at the same time");
            }

            var builder = new HttpQueryStringBuilder();
            builder.Add("q", query);
            builder.AddProperties(properties);
            builder.Add("count", count.ToString());

            if (contactOffset.HasValue)
            {
                builder.Add("offset", contactOffset.Value.ToString());
            }

            var response = await SendAsync<SearchResponse>(HttpMethod.Get, "/contacts/v1/search/query", builder.BuildQuery());

            return response;
        }

        async Task IHubSpotContactClient.MergeAsync(long primaryContactId, long secondaryContactId)
        {
            var payload = new
            {
                vidToMerge = secondaryContactId
            };

            await SendAsync<object>(HttpMethod.Post, payload, $"/contacts/v1/contact/merge-vids/{primaryContactId}/");
        }
    }
}