using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.NUnit3;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;


namespace Tests.Contacts
{
    [TestFixture]
    public class CreateOrUpdateByEmailAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactClient sut, string email, IReadOnlyList<ValuedProperty> properties, CreateOrUpdateResponse response)
        {
            Mock.Get(client)
                .Setup(p => p.SendAsync<PropertyList<ValuedProperty>, CreateOrUpdateResponse>(HttpMethod.Post, It.Is<string>(s => s.StartsWith($"/contacts/v1/contact/createOrUpdate/email/")), It.IsAny<PropertyList<ValuedProperty>>(), null))
                .ReturnsAsync(response);

            await sut.CreateOrUpdateByEmailAsync(email, properties);

            Mock.Get(client)
                .Verify(p => p.SendAsync<PropertyList<ValuedProperty>, CreateOrUpdateResponse>(HttpMethod.Post, $"/contacts/v1/contact/createOrUpdate/email/{email}", PropertyList.Contains(properties), null));
        }
    }
}