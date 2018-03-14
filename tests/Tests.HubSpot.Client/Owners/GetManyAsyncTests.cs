using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Model.Owners;
using Kralizek.Extensions.Http;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Owners
{
    [TestFixture]
    public class GetManyAsyncTests : OwnerTests
    {
        [Test, AutoData]
        public async Task Email_address_is_appended_to_query_to_filter_by_email(Owner owner)
        {
            var options = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Get,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.FromObject(new[] { owner }, HttpHubSpotClient.SerializerSettings)
                }
            };

            var sut = CreateSystemUnderTest(options);

            var response = await sut.GetManyAsync(owner.Email);

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.Query, Contains.Substring($"email={owner.Email}"));
        }

        [Test, AutoData]
        public async Task Several_contacts_are_returned(Owner[] owners)
        {
            var options = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Get,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.FromObject(owners, HttpHubSpotClient.SerializerSettings)
                }
            };

            var sut = CreateSystemUnderTest(options);

            var response = await sut.GetManyAsync();

            Assert.That(options.HttpResponseMessage.RequestMessage.RequestUri.Query, Does.Not.Contain("email"));
        }
    }
}