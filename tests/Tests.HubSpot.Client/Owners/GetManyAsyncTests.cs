using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot.Model.Owners;
using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;

namespace Tests.Owners
{
    [TestFixture]
    public class GetManyAsyncTests
    {
        [Test, CustomAutoData]
        public async Task Email_address_is_appended_to_query_to_filter_by_email([Frozen] IHttpRestClient client, IHubSpotOwnerClient sut, Owner owner)
        {
            Mock.Get(client)
                .Setup(p => p.SendAsync<IReadOnlyList<Owner>>(HttpMethod.Get, "/owners/v2/owners/", It.IsAny<IQueryString>()))
                .ReturnsAsync(new[]{owner});

            var response = await sut.GetManyAsync(owner.Email);

            Mock.Get(client).Verify(p => p.SendAsync<IReadOnlyList<Owner>>(HttpMethod.Get, "/owners/v2/owners/", QueryStringMatcher.That(Contains.Substring($"email={owner.Email}"))));
        }

        [Test, CustomAutoData]
        public async Task Several_contacts_are_returned([Frozen] IHttpRestClient client, IHubSpotOwnerClient sut, Owner[] owners)
        {
            Mock.Get(client)
                .Setup(p => p.SendAsync<IReadOnlyList<Owner>>(HttpMethod.Get, "/owners/v2/owners/", It.IsAny<IQueryString>()))
                .ReturnsAsync(owners);

            var response = await sut.GetManyAsync();

            Mock.Get(client).Verify(p => p.SendAsync<IReadOnlyList<Owner>>(HttpMethod.Get, "/owners/v2/owners/", QueryStringMatcher.That(Does.Not.Contain("email"))));
        }
    }
}