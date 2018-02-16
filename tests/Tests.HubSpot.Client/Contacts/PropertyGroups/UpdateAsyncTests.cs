using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot.Model.Contacts.Properties;
using Kralizek.Extensions.Http;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Contacts.PropertyGroups
{
    [TestFixture]
    public class UpdateAsyncTests : ContactPropertyGroupTests
    {
        [Test, AutoData]
        public async Task Request_is_correct(string groupName, ContactPropertyGroup @group)
        {
            var option = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Put,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.EmptyObject
                }
            };

            var sut = CreateSystemUnderTest(option);

            var response = await sut.UpdateAsync(groupName, @group);

            Assert.That(option.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring($"/properties/v1/contacts/groups/named/{groupName}"));
        }
    }
}