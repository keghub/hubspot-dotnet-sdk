using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot.Model.Contacts.Properties;
using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;


namespace Tests.Contacts.PropertyGroups
{
    [TestFixture]
    public class CreateAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactPropertyGroupClient sut, ContactPropertyGroup @group)
        {
            var response = await sut.CreateAsync(group);

            Mock.Get(client)
                .Verify(p => p.SendAsync<ContactPropertyGroup, ContactPropertyGroup>(HttpMethod.Post, "/properties/v1/contacts/groups", group, null));
        }
    }
}