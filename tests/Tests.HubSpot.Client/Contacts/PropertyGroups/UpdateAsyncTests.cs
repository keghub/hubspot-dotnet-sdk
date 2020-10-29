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
    public class UpdateAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactPropertyGroupClient sut, string groupName, ContactPropertyGroup @group)
        {
            var response = await sut.UpdateAsync(groupName, @group);

            Mock.Get(client)
                .Verify(p => p.SendAsync<ContactPropertyGroup, ContactPropertyGroup>(HttpMethod.Put, $" /properties/v1/contacts/groups/named/{groupName}", group, null));
        }
    }
}