using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using HubSpot.Model.Lists;
using Kralizek.Extensions.Http;

namespace HubSpot
{
    public partial class HttpHubSpotClient : IHubSpotListClient
    {
        async Task<List> IHubSpotListClient.CreateAsync(string name, bool dynamic, IReadOnlyList<IReadOnlyList<Filter>> filters)
        {
            var request = new
            {
                name,
                dynamic,
                filters
            };

            var response = await _client.PostAsync<object, List>("/contacts/v1/lists", request);

            return response;
        }

        async Task<ListList> IHubSpotListClient.GetAllAsync(int count, long? offset)
        {
            if (count > 250)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Up to 250 lists can be requested at the same time");
            }

            var builder = new HttpQueryStringBuilder();
            builder.Add("count", count);
            builder.Add("offset", offset);

            var response = await _client.GetAsync<ListList>("/contacts/v1/lists", builder.BuildQuery());

            return response;
        }

        async Task<List> IHubSpotListClient.GetByIdAsync(long listId)
        {
            var response = await _client.GetAsync<List>($"/contacts/v1/lists/{listId}");

            return response;
        }

        async Task<List> IHubSpotListClient.UpdateAsync(long listId, string name, bool? dynamic, IReadOnlyList<IReadOnlyList<Filter>> filters)
        {
            var request = new
            {
                name,
                dynamic,
                filters
            };

            var response = await _client.PostAsync<object, List>($"/contacts/v1/lists/{listId}", request);

            return response;
        }

        async Task IHubSpotListClient.DeleteAsync(long listId)
        {
            await _client.DeleteAsync($"/contacts/v1/lists/{listId}");
        }

        async Task<ListList> IHubSpotListClient.GetManyByIdAsync(IReadOnlyList<long> listIds)
        {
            if (listIds == null || listIds.Count == 0)
            {
                return ListList.Empty;
            }

            var builder = new HttpQueryStringBuilder();

            foreach (var id in listIds)
            {
                builder.Add("listId", id);
            }

            var response = await _client.GetAsync<ListList>("/contacts/v1/lists/batch", builder.BuildQuery());

            return response;
        }

        async Task<ListList> IHubSpotListClient.GetAllStaticAsync(int count, long? offset)
        {
            if (count > 250)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Up to 250 lists can be requested at the same time");
            }

            var builder = new HttpQueryStringBuilder();
            builder.Add("count", count);
            builder.Add("offset", offset);

            var response = await _client.GetAsync<ListList>("/contacts/v1/lists/static", builder.BuildQuery());

            return response;
        }

        async Task<ListList> IHubSpotListClient.GetAllDynamicAsync(int count, long? offset)
        {
            if (count > 250)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Up to 250 lists can be requested at the same time");
            }

            var builder = new HttpQueryStringBuilder();
            builder.Add("count", count);
            builder.Add("offset", offset);

            var response = await _client.GetAsync<ListList>("/contacts/v1/lists/dynamic", builder.BuildQuery());

            return response;
        }

        async Task<ContactList> IHubSpotListClient.GetContactsInListAsync(long listId, IReadOnlyList<IProperty> properties, PropertyMode propertyMode, FormSubmissionMode formSubmissionMode, bool showListMemberships, int count, long? contactOffset)
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
            builder.Add("vidOffset", contactOffset);

            var list = await _client.GetAsync<ContactList>($"/contacts/v1/lists/{listId}/contacts/all", builder.BuildQuery());

            return list;
        }

        async Task<ContactList> IHubSpotListClient.GetContactsRecentlyAddedToListAsync(long listId, IReadOnlyList<IProperty> properties, PropertyMode propertyMode, FormSubmissionMode formSubmissionMode, bool showListMemberships, int count, long? contactOffset, DateTimeOffset? timeOffset)
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
            builder.Add("vidOffset", contactOffset);

            if (timeOffset.HasValue)
            {
                builder.Add("timeOffset", timeOffset.Value.ToUnixTimeMilliseconds().ToString());
            }

            var list = await _client.GetAsync<ContactList>($"/contacts/v1/lists/{listId}/contacts/recent", builder.BuildQuery());

            return list;
        }

        async Task<ContactListResponse> IHubSpotListClient.AddContactsToListAsync(long listId, IReadOnlyList<long> contactIds, IReadOnlyList<string> contactEmails)
        {
            var request = new
            {
                vids = contactIds,
                emails = contactEmails
            };

            var response = await _client.PostAsync<object, ContactListResponse>($"/contacts/v1/lists/{listId}/add", request);

            return response;
        }

        async Task<ContactListResponse> IHubSpotListClient.RemoveContactFromListAsync(long listId, long contactId)
        {
            var request = new
            {
                vids = new[] { contactId }
            };

            var response = await _client.PostAsync<object, ContactListResponse>($"/contacts/v1/lists/{listId}/remove", request);

            return response;
        }
    }
}