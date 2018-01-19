using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HubSpot.Model.Contacts
{
    public interface IHubSpotContactClient
    {
        Task<Contact> GetByIdAsync(long contactId, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueAndHistory, FormSubmissionMode formSubmissionMode = FormSubmissionMode.All, bool showListMemberships = true);

        Task<Contact> GetByEmailAsync(string email, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueAndHistory, FormSubmissionMode formSubmissionMode = FormSubmissionMode.All, bool showListMemberships = true);

        Task<Contact> GetByUserTokenAsync(string userToken, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueAndHistory, FormSubmissionMode formSubmissionMode = FormSubmissionMode.All, bool showListMemberships = true);

        Task<IReadOnlyDictionary<long, Contact>> GetManyContactsByIdAsync(IReadOnlyList<long> contactIds, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, bool includeDeletes = false);

        Task<IReadOnlyDictionary<long, Contact>> GetManyByEmailAsync(IReadOnlyList<string> emails, IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, bool includeDeletes = false);

        Task<ContactList> GetAllAsync(IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, int count = 20, long? contactOffset = null);

        Task<ContactList> GetRecentlyUpdatedAsync(IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, int count = 20, long? contactOffset = null, DateTimeOffset? timeOffset = null);

        Task<ContactList> GetRecentlyCreatedAsync(IReadOnlyList<IProperty> properties = null, PropertyMode propertyMode = PropertyMode.ValueOnly, FormSubmissionMode formSubmissionMode = FormSubmissionMode.Newest, bool showListMemberships = false, int count = 20, long? contactOffset = null, DateTimeOffset? timeOffset = null);

        Task<DeleteContactResponse> DeleteAsync(long contactId);

        Task<Contact> CreateAsync(IReadOnlyList<ValuedProperty> properties);

        Task UpdateByIdAsync(long contactId, IReadOnlyList<ValuedProperty> properties);

        Task UpdateByEmailAsync(string email, IReadOnlyList<ValuedProperty> properties);

        Task<CreateOrUpdateResponse> CreateOrUpdateByEmailAsync(string email, IReadOnlyList<ValuedProperty> properties);
    }

    public enum PropertyMode
    {
        ValueOnly = 1,
        ValueAndHistory = 2
    }

    public enum FormSubmissionMode
    {
        None = 0,
        Oldest = 1,
        Newest = 2,
        All = 3
    }


}