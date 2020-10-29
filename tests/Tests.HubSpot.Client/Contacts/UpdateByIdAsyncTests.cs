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
    public class UpdateByIdAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Request_is_correct([Frozen] IHttpRestClient client, IHubSpotContactClient sut, long contactId, IReadOnlyList<ValuedProperty> properties)
        {
            await sut.UpdateByIdAsync(contactId, properties);

            Mock.Get(client)
                .Verify(p => p.SendAsync(HttpMethod.Post, $"/contacts/v1/contact/vid/{contactId}/profile", PropertyList.Contains(properties), null));
        }
    }
}