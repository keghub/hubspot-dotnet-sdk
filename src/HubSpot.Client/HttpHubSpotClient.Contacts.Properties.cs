using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HubSpot.Model.Contacts.Properties;

namespace HubSpot
{
    public partial class HttpHubSpotClient : IHubSpotContactPropertyClient, IHubSpotContactPropertyGroupClient
    {
        async Task<IReadOnlyList<ContactPropertyGroup>> IHubSpotContactPropertyGroupClient.GetAllAsync()
        {
            return await SendAsync<ContactPropertyGroup[]>(HttpMethod.Get, "/properties/v1/contacts/groups");
        }

        async Task<ContactPropertyGroup> IHubSpotContactPropertyGroupClient.CreateAsync(ContactPropertyGroup group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            if (group.Name == null)
            {
                throw new ArgumentNullException(nameof(group.Name));
            }

            if (group.DisplayName == null)
            {
                throw new ArgumentNullException(nameof(group.DisplayName));
            }

            return await SendAsync<ContactPropertyGroup, ContactPropertyGroup>(HttpMethod.Post, "/properties/v1/contacts/groups", group);
        }

        async Task<ContactPropertyGroup> IHubSpotContactPropertyGroupClient.UpdateAsync(string groupName, ContactPropertyGroup group)
        {
            if (groupName == null)
            {
                throw new ArgumentNullException(nameof(groupName));
            }

            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            if (group.Name == null)
            {
                throw new ArgumentNullException(nameof(group.Name));
            }

            if (group.DisplayName == null)
            {
                throw new ArgumentNullException(nameof(group.DisplayName));
            }

            return await SendAsync<ContactPropertyGroup, ContactPropertyGroup>(HttpMethod.Put, $" /properties/v1/contacts/groups/named/{groupName}", group);
        }

        async Task IHubSpotContactPropertyGroupClient.DeleteAsync(string groupName)
        {
            if (groupName == null)
            {
                throw new ArgumentNullException(nameof(groupName));
            }

            await SendAsync(HttpMethod.Delete, $"/properties/v1/contacts/groups/named/{groupName}");
        }

        async Task<IReadOnlyList<ContactProperty>> IHubSpotContactPropertyClient.GetAllAsync()
        {
            return await SendAsync<ContactProperty[]>(HttpMethod.Get, "/properties/v1/contacts/properties");
        }

        async Task<ContactProperty> IHubSpotContactPropertyClient.GetByNameAsync(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            return await SendAsync<ContactProperty>(HttpMethod.Get, $"/properties/v1/contacts/properties/named/{propertyName}");
        }

        async Task<ContactProperty> IHubSpotContactPropertyClient.CreateAsync(ContactProperty propertyToCreate)
        {
            if (propertyToCreate == null)
            {
                throw new ArgumentNullException(nameof(propertyToCreate));
            }

            if (propertyToCreate.Name == null)
            {
                throw new ArgumentNullException(nameof(propertyToCreate.Name));
            }

            if (propertyToCreate.Label == null)
            {
                throw new ArgumentNullException(nameof(propertyToCreate.Label));
            }

            if (propertyToCreate.GroupName == null)
            {
                throw new ArgumentNullException(nameof(propertyToCreate.GroupName));
            }

            var property = await SendAsync<ContactProperty, ContactProperty>(HttpMethod.Post, "/properties/v1/contacts/properties", propertyToCreate);

            return property;
        }

        async Task<ContactProperty> IHubSpotContactPropertyClient.UpdateAsync(string propertyName, ContactProperty updatedProperty)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (updatedProperty == null)
            {
                throw new ArgumentNullException(nameof(updatedProperty));
            }

            if (updatedProperty.Name == null)
            {
                throw new ArgumentNullException(nameof(updatedProperty.Name));
            }

            var property = await SendAsync<ContactProperty, ContactProperty>(HttpMethod.Put, $"/properties/v1/contacts/properties/named/{propertyName}", updatedProperty);

            return property;
        }

        async Task IHubSpotContactPropertyClient.DeleteAsync(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            await SendAsync(HttpMethod.Delete, $"/properties/v1/contacts/properties/named/{propertyName}");
        }
    }
}