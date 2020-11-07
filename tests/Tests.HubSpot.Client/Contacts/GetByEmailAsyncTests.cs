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
    public class GetByEmailAsyncTests
    {
        [Test, CustomAutoData]
        public void Email_is_required(IHubSpotContactClient sut)
        {
            Assert.That(() => sut.GetByEmailAsync(null), Throws.ArgumentNullException);
        }

        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactClient sut, string email)
        {
            var response = await sut.GetByEmailAsync(email);

            Mock.Get(client).Verify(p => p.SendAsync<Contact>(HttpMethod.Get, $"/contacts/v1/contact/email/{email}/profile", It.IsAny<IQueryString>()));
        }
    }
}