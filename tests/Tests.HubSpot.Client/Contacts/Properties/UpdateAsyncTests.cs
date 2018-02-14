using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using HubSpot.Model.Contacts.Properties;
using NUnit.Framework;
using WorldDomination.Net.Http;

namespace Tests.Contacts.Properties
{
    [TestFixture]
    public class UpdateAsyncTests : ContactPropertyTests
    {
        [Test, AutoData]
        public async Task Request_is_correct(string propertyName, ContactProperty property)
        {
            var option = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Put,
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = EmptyObject
                }
            };

            var sut = CreateSystemUnderTest(option);

            var response = await sut.UpdateAsync(propertyName, property);

            Assert.That(option.HttpResponseMessage.RequestMessage.RequestUri.AbsolutePath, Contains.Substring($"/properties/v1/contacts/properties/named/{propertyName}"));
        }
    }
}