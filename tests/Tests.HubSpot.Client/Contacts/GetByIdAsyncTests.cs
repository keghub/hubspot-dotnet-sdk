using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Tests.Contacts
{
    [TestFixture]
    public class GetByIdAsyncTests
    {
        [Test]
        [CustomAutoData]
        public void NotFoundException_is_thrown_if_404([Frozen] IHttpRestClient client, IHubSpotContactClient sut, long contactId)
        {
            Mock.Get(client)
                .Setup(p => p.SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/vid/{contactId}/profile", It.IsAny<IQueryString>()))
                .Throws(new HttpException("Not Found", HttpStatusCode.NotFound));

            Assert.That(() => sut.GetByIdAsync(contactId), Throws.InstanceOf<NotFoundException>());
        }

        [Test]
        [CustomAutoData]
        public async Task Request_absolutePath_is_correct([Frozen] IHttpRestClient client, IHubSpotContactClient sut, long contactId, Contact contact)
        {
            Mock.Get(client)
                .Setup(p => p.SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/vid/{contactId}/profile", It.IsAny<IQueryString>()))
                .ReturnsAsync(contact)
                .Verifiable();

            var response = await sut.GetByIdAsync(contactId);

            Mock.Verify();
        }

        [Test, CustomAutoData]
        public async Task Properties_are_added_to_queryString([Frozen] IHttpRestClient client, IHubSpotContactClient sut, long contactId, Property[] properties, Contact contact)
        {
            Mock.Get(client)
                .Setup(p => p.SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/vid/{contactId}/profile", It.IsAny<IQueryString>()))
                .ReturnsAsync(contact)
                .Verifiable();

            var response = await sut.GetByIdAsync(contactId, properties);

            Mock.Get(client)
                .Verify(p => p.SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/vid/{contactId}/profile", QueryStringMatcher.That(NUnitHelpers.All(properties, property => Does.Contain($"property={property.Name}")))));
        }

        [Test]
        [InlineCustomAutoData(PropertyMode.ValueAndHistory, "value_and_history")]
        [InlineCustomAutoData(PropertyMode.ValueOnly, "value_only")]
        public async Task PropertyMode_is_added_to_queryString(PropertyMode propertyMode, string queryStringValue, [Frozen] IHttpRestClient client, IHubSpotContactClient sut, long contactId, Contact contact)
        {
            Mock.Get(client)
                .Setup(p => p.SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/vid/{contactId}/profile", It.IsAny<IQueryString>()))
                .ReturnsAsync(contact)
                .Verifiable();

            var response = await sut.GetByIdAsync(contactId, propertyMode: propertyMode);

            Mock.Get(client)
                .Verify(p => p.SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/vid/{contactId}/profile", QueryStringMatcher.That(Does.Contain($"propertyMode={queryStringValue}"))));
        }

        [Test]
        [InlineCustomAutoData(FormSubmissionMode.All, "all")]
        [InlineCustomAutoData(FormSubmissionMode.Newest, "newest")]
        [InlineCustomAutoData(FormSubmissionMode.Oldest, "oldest")]
        [InlineCustomAutoData(FormSubmissionMode.None, "none")]
        public async Task FormSubmissionMode_is_added_to_queryString(FormSubmissionMode formSubmissionMode, string queryStringValue, [Frozen] IHttpRestClient client, IHubSpotContactClient sut, long contactId, Contact contact)
        {
            Mock.Get(client)
                .Setup(p => p.SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/vid/{contactId}/profile", It.IsAny<IQueryString>()))
                .ReturnsAsync(contact)
                .Verifiable();

            var response = await sut.GetByIdAsync(contactId, formSubmissionMode: formSubmissionMode);

            Mock.Get(client)
                .Verify(p => p.SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/vid/{contactId}/profile", QueryStringMatcher.That(Does.Contain($"formSubmissionMode={queryStringValue}"))));
        }

        [Test]
        [InlineCustomAutoData(true, "true")]
        [InlineCustomAutoData(false, "false")]
        public async Task ShowListMemberships_is_added_to_queryString(bool showListMemberships, string queryStringValue, [Frozen] IHttpRestClient client, IHubSpotContactClient sut, long contactId, Contact contact)
        {
            Mock.Get(client)
                .Setup(p => p.SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/vid/{contactId}/profile", It.IsAny<IQueryString>()))
                .ReturnsAsync(contact)
                .Verifiable();

            var response = await sut.GetByIdAsync(contactId, showListMemberships: showListMemberships);

            Mock.Get(client)
                .Verify(p => p.SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/vid/{contactId}/profile", QueryStringMatcher.That(Does.Contain($"showListMemberships={queryStringValue}"))));
        }
    }
}