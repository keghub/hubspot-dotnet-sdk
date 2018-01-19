using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HubSpot.Model.Contacts;

namespace HubSpot
{
    public partial class HttpHubSpotClient : IHubSpotContactClient
    {
        public Task<Contact> GetByIdAsync(long contactId, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueAndHistory, FormSubmissionMode formSubmissionMode = FormSubmissionMode.All, bool showListMemberships = true)
        {
            throw new NotImplementedException();
        }

        public Task<Contact> GetByEmailAsync(string email, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueAndHistory, FormSubmissionMode formSubmissionMode = FormSubmissionMode.All, bool showListMemberships = true)
        {
            throw new NotImplementedException();
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