using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Contacts.PropertyGroups
{
    [TestFixture]
    public class DeleteAsyncTests : ContactPropertyGroupTests
    {
        [Test, AutoData]
        public async Task Request_is_correct(string groupName)
        {
            var option = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Delete,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = EmptyObject
                }
            };

            var sut = CreateSystemUnderTest(option);

            await sut.DeleteAsync(groupName);

            Assert.That(option.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring($"/properties/v1/contacts/groups/named/{groupName}"));
        }
    }
}