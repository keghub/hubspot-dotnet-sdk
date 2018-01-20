using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using HubSpot.Model.Contacts;
using HubSpot.Utils;

namespace HubSpot
{
    public static class HttpQueryStringBuilderContactExtensions
    {
        public static void AddProperties(this HttpQueryStringBuilder builder, IEnumerable<IProperty> properties)
        {
            foreach (var property in properties ?? Array.Empty<IProperty>())
            {
                builder.Add("property", property.Name);
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


        public async Task<Contact> GetByIdAsync(long contactId, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueAndHistory, FormSubmissionMode formSubmissionMode = FormSubmissionMode.All, bool showListMemberships = true)
        {
            var builder = new HttpQueryStringBuilder();

            builder.AddProperties(properties);
            builder.AddPropertyMode(propertyMode);
            builder.AddFormSubmissionMode(formSubmissionMode);
            builder.AddShowListMemberships(showListMemberships);

            var contact = await SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/vid/{contactId}/profile", builder.BuildQuery());

            return contact;

        }

        public async Task<Contact> GetByEmailAsync(string email, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueAndHistory, FormSubmissionMode formSubmissionMode = FormSubmissionMode.All, bool showListMemberships = true)
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

        public Task<Contact> GetByUserTokenAsync(string userToken, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueAndHistory, FormSubmissionMode formSubmissionMode = FormSubmissionMode.All, bool showListMemberships = true)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyDictionary<long, Contact>> GetManyContactsByIdAsync(IReadOnlyList<long> contactIds, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, bool includeDeletes = false)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyDictionary<long, Contact>> GetManyByEmailAsync(IReadOnlyList<string> emails, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, bool includeDeletes = false)
        {
            throw new NotImplementedException();
        }

        public Task<ContactList> GetAllAsync(IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, int count = 20, long? contactOffset = null)
        {
            throw new NotImplementedException();
        }

        public Task<ContactList> GetRecentlyUpdatedAsync(IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, int count = 20, long? contactOffset = null, DateTimeOffset? timeOffset = null)
        {
            throw new NotImplementedException();
        }

        public Task<ContactList> GetRecentlyCreatedAsync(IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, int count = 20, long? contactOffset = null, DateTimeOffset? timeOffset = null)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteContactResponse> DeleteAsync(long contactId)
        {
            throw new NotImplementedException();
        }

        public Task<Contact> CreateAsync(IReadOnlyList<ValuedProperty> properties)
        {
            throw new NotImplementedException();
        }

        public Task UpdateByIdAsync(long contactId, IReadOnlyList<ValuedProperty> properties)
        {
            throw new NotImplementedException();
        }

        public Task UpdateByEmailAsync(string email, IReadOnlyList<ValuedProperty> properties)
        {
            throw new NotImplementedException();
        }

        public Task<CreateOrUpdateResponse> CreateOrUpdateByEmailAsync(string email, IReadOnlyList<ValuedProperty> properties)
        {
            throw new NotImplementedException();
        }
    }
}