using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HubSpot.Model.Contacts.Properties;
using Kralizek.Extensions.Http;

namespace HubSpot
{
    public partial class HttpHubSpotClient : IHubSpotContactPropertyClient, IHubSpotContactPropertyGroupClient
    {
        async Task<IReadOnlyList<ContactPropertyGroup>> IHubSpotContactPropertyGroupClient.GetAllAsync()
        {
            return await _client.SendAsync<ContactPropertyGroup[]>(HttpMethod.Get, "/properties/v1/contacts/groups", null);
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

            return await _client.SendAsync<ContactPropertyGroup, ContactPropertyGroup>(HttpMethod.Post, "/properties/v1/contacts/groups", @group, null);
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

            string path = $" /properties/v1/contacts/groups/named/{groupName}";
            return await _client.SendAsync<ContactPropertyGroup, ContactPropertyGroup>(HttpMethod.Put, path, @group, null);
        }

        async Task IHubSpotContactPropertyGroupClient.DeleteAsync(string groupName)
        {
            if (groupName == null)
            {
                throw new ArgumentNullException(nameof(groupName));
            }

            string path = $"/properties/v1/contacts/groups/named/{groupName}";
            await _client.SendAsync(HttpMethod.Delete, path, null);
        }

        async Task<IReadOnlyList<ContactProperty>> IHubSpotContactPropertyClient.GetAllAsync()
        {
            return await _client.SendAsync<ContactProperty[]>(HttpMethod.Get, "/properties/v1/contacts/properties", null);
        }

        async Task<ContactProperty> IHubSpotContactPropertyClient.GetByNameAsync(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            string path = $"/properties/v1/contacts/properties/named/{propertyName}";
            return await _client.SendAsync<ContactProperty>(HttpMethod.Get, path, null);
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

            var property = await _client.SendAsync<ContactProperty, ContactProperty>(HttpMethod.Post, "/properties/v1/contacts/properties", propertyToCreate, null);

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

            string path = $"/properties/v1/contacts/properties/named/{propertyName}";
            var property = await _client.SendAsync<ContactProperty, ContactProperty>(HttpMethod.Put, path, updatedProperty, null);

            return property;
        }

        async Task IHubSpotContactPropertyClient.DeleteAsync(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            string path = $"/properties/v1/contacts/properties/named/{propertyName}";
            await _client.SendAsync(HttpMethod.Delete, path, null);
        }
    }
}