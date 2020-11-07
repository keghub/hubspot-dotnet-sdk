using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot.Model.Contacts;
using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;


namespace Tests.Contacts
{
    [TestFixture]
    public class GetManyByEmailAsyncTests
    {
        [Test]
        [CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactClient sut, string[] emails, Dictionary<long, Contact> contacts)
        {
            Mock.Get(client)
                .Setup(p => p.SendAsync<Dictionary<long, Contact>>(HttpMethod.Get, "/contacts/v1/contact/emails/batch/", It.IsAny<IQueryString>()))
                .ReturnsAsync(contacts)
                .Verifiable();

            var response = await sut.GetManyByEmailAsync(emails);

            Mock.Verify();
        }

        [Test]
        [CustomAutoData]
        public async Task Emails_are_attached_to_request([Frozen] IHttpRestClient client, IHubSpotContactClient sut, string[] emails, Dictionary<long, Contact> contacts)
        {
            Mock.Get(client)
                .Setup(p => p.SendAsync<Dictionary<long, Contact>>(HttpMethod.Get, "/contacts/v1/contact/emails/batch/", It.IsAny<IQueryString>()))
                .ReturnsAsync(contacts);

            var response = await sut.GetManyByEmailAsync(emails);

            Mock.Get(client)
                .Verify(p => p.SendAsync<Dictionary<long, Contact>>(HttpMethod.Get, "/contacts/v1/contact/emails/batch/", QueryStringMatcher.That(NUnitHelpers.All(emails, email => Does.Contain($"email={email}")))));
        }

        [Test]
        [CustomAutoData]
        public async Task Returns_empty_dictionary_if_no_id(IHubSpotContactClient sut)
        {
            var response = await sut.GetManyByEmailAsync(Array.Empty<string>());

            Assert.That(response, Is.Empty);
        }

        [Test]
        [CustomAutoData]
        public async Task Returns_empty_dictionary_if_null(IHubSpotContactClient sut)
        {
            var response = await sut.GetManyByEmailAsync(null);

            Assert.That(response, Is.Empty);
        }

        [Test]
        [InlineCustomAutoData(201)]
        public void Throws_OutOfRange_if_more_than_100(int count, IHubSpotContactClient sut)
        {
            var emails = new string[count];

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await sut.GetManyByEmailAsync(emails));
        }
    }
}