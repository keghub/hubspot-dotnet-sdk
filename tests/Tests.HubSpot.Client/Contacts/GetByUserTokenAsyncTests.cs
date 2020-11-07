using System;
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
    public class GetByUserTokenAsyncTests
    {
        [Test]
        [CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactClient sut, string userToken, Contact contact)
        {
            Mock.Get(client)
                .Setup(p => p.SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/utk/{userToken}/profile", It.IsAny<IQueryString>()))
                .ReturnsAsync(contact)
                .Verifiable();

            var response = await sut.GetByUserTokenAsync(userToken);

            Mock.Verify();
        }

        [Test]
        [CustomAutoData]
        public void UserToken_is_required(IHubSpotContactClient sut)
        {
            Assert.That(() => sut.GetByUserTokenAsync(null), Throws.ArgumentNullException);
        }
    }
}