using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot.Model;
using HubSpot.Model.Contacts;
using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;


namespace Tests.Contacts
{
    [TestFixture]
    public class CreateAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactClient sut, Contact contact)
        {
            var properties = (from p in contact.Properties
                              select new ValuedProperty(p.Key, p.Value.Value)).ToArray();

            Mock.Get(client)
                .Setup(p => p.SendAsync<PropertyList<ValuedProperty>, Contact>(HttpMethod.Post, "/contacts/v1/contact", It.IsAny<PropertyList<ValuedProperty>>(), null))
                .ReturnsAsync(contact)
                .Verifiable();

            var response = await sut.CreateAsync(properties);

            Mock.Get(client).Verify();
        }
    }
}