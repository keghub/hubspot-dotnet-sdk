using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Model;
using HubSpot.Model.LineItems;
using HubSpot.Model.Pipelines;
using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Pipelines
{
    [TestFixture]
    public class HubSpotLineItemClientTests
    {
        [Test, CustomAutoData]
        public void Constructor_is_guarded_against_null_parameters(GuardClauseAssertion guardClauseAssertion)
        {
            //Arrange

            //Act

            //Assert
            guardClauseAssertion.Verify(typeof(HttpHubSpotClient).GetConstructors());
        }

        #region GetAsync

        [Test, CustomAutoData]
        public void GetAsync_throws_NotFoundException_if_request_returns_NotFound_status([Frozen] IHttpRestClient httpRestClient, IHubSpotLineItemClient sut, IReadOnlyList<IProperty> properties, long lineItemId, IFixture fixture)
        {
            var httpException = fixture.Build<HttpException>().With(x => x.StatusCode, HttpStatusCode.NotFound).Create();
            Mock.Get(httpRestClient)
                .Setup(p => p.SendAsync<LineItem>(HttpMethod.Get, $"/crm/v3/objects/line_items/{lineItemId}", It.IsAny<IQueryString>()))
                .Throws(httpException);

            Assert.That(() => sut.GetAsync(lineItemId, properties), Throws.InstanceOf<NotFoundException>());
        }

        [Test, CustomAutoData]
        public async Task GetAsync_invokes_http_client_get_method_once_with_id([Frozen] IHttpRestClient httpRestClient, IHubSpotLineItemClient sut, long lineItemId, IReadOnlyList<IProperty> properties)
        {
            //Arrange

            //Act
            await sut.GetAsync(lineItemId, properties);

            //Assert
            Mock.Get(httpRestClient).Verify(p => p.SendAsync<LineItem>(HttpMethod.Get, $"/crm/v3/objects/line_items/{lineItemId}", It.IsAny<IQueryString>()), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task GetAsync_returns_null_if_API_retrieves_null([Frozen] IHttpRestClient httpRestClient, IHubSpotLineItemClient sut, long lineItemId, IReadOnlyList<IProperty> properties)
        {
            //Arrange
            Mock.Get(httpRestClient)
                .Setup(p => p.SendAsync<LineItem>(HttpMethod.Get, $"/crm/v3/objects/line_items/{lineItemId}", It.IsAny<IQueryString>())).Returns(Task.FromResult((LineItem)null));

            //Act
            var result = await sut.GetAsync(lineItemId, properties);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test, CustomAutoData]
        public async Task GetAsync_returns_lineitem_retrieved_by_http_client_request([Frozen] IHttpRestClient httpRestClient,[Frozen] LineItem lineItem, IHubSpotLineItemClient sut, long lineItemId,
            IReadOnlyList<IProperty> properties)
        {
            //Arrange
            Mock.Get(httpRestClient)
                .Setup(p => p.SendAsync<LineItem>(HttpMethod.Get, $"/crm/v3/objects/line_items/{lineItemId}", It.IsAny<IQueryString>())).Returns(Task.FromResult(lineItem));

            //Act
            var result = await sut.GetAsync(lineItemId, properties);

            //Assert
            Assert.That(result, Is.EqualTo(lineItem));
        }

        [Test, CustomAutoData]
        public async Task GetAsync_returns_lineitem_with_valid_id([Frozen] IHttpRestClient httpRestClient, IHubSpotLineItemClient sut, long lineItemId, IFixture fixture,
            IReadOnlyList<IProperty> properties)
        {
            //Arrange
            var lineItem = fixture.Build<LineItem>().With(x => x.Id, lineItemId).Create();
            Mock.Get(httpRestClient)
                .Setup(p => p.SendAsync<LineItem>(HttpMethod.Get, $"/crm/v3/objects/line_items/{lineItemId}", It.IsAny<IQueryString>())).Returns(Task.FromResult(lineItem));

            //Act
            var result = await sut.GetAsync(lineItemId, properties);

            //Assert
            Assert.That(result.Id, Is.EqualTo(lineItemId));
        }

        #endregion

        #region GetBySKUAsync
        [Test, CustomAutoData]
        public void GetBySKUAsync_throws_NotFoundException_if_request_returns_NotFound_status([Frozen] IHttpRestClient httpRestClient, IHubSpotLineItemClient sut, IReadOnlyList<IProperty> properties, IReadOnlyList<long> lineItemIds, string sku, IFixture fixture)
        {
            var httpException = fixture.Build<HttpException>().With(x => x.StatusCode, HttpStatusCode.NotFound).Create();
            Mock.Get(httpRestClient)
                .Setup(p => p.SendAsync<object, LineItemResult>(HttpMethod.Post, $"/crm/v3/objects/line_items/search", It.IsAny<object>(), null))
                .Throws(httpException);

            Assert.That(() => sut.GetBySKUAsync(lineItemIds, properties, sku), Throws.InstanceOf<NotFoundException>());
        }

        [Test, CustomAutoData]
        public async Task GetBySKUAsync_invokes_http_client_get_method_once_with_id([Frozen] IHttpRestClient httpRestClient, IHubSpotLineItemClient sut, IReadOnlyList<IProperty> properties, IReadOnlyList<long> lineItemIds, string sku, IFixture fixture)
        {
            //Arrange

            //Act
            await sut.GetBySKUAsync(lineItemIds, properties, sku);

            //Assert
            Mock.Get(httpRestClient).Verify(p => p.SendAsync<object, LineItemResult>(HttpMethod.Post, $"/crm/v3/objects/line_items/search", It.IsAny<object>(), null), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task GetBySKUAsync_returns_null_if_API_retrieves_null([Frozen] IHttpRestClient httpRestClient, IHubSpotLineItemClient sut, IReadOnlyList<IProperty> properties, 
            IReadOnlyList<long> lineItemIds, string sku)
        {
            //Arrange
            Mock.Get(httpRestClient)
                .Setup(p => p.SendAsync<object, LineItemResult>(HttpMethod.Post, $"/crm/v3/objects/line_items/search", It.IsAny<object>(), null)).Returns(Task.FromResult((LineItemResult)null));

            //Act
            var result = await sut.GetBySKUAsync(lineItemIds, properties, sku);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test, CustomAutoData]
        public async Task GetBySKUAsync_returns_lineitem_retrieved_by_http_client_request([Frozen] IHttpRestClient httpRestClient, [Frozen] LineItemResult lineItem, IHubSpotLineItemClient sut, IReadOnlyList<long> lineItemIds,
            IReadOnlyList<IProperty> properties, string sku)
        {
            //Arrange
            Mock.Get(httpRestClient)
                .Setup(p => p.SendAsync<object, LineItemResult>(HttpMethod.Post, $"/crm/v3/objects/line_items/search", It.IsAny<object>(), null)).Returns(Task.FromResult(lineItem));

            //Act
            var result = await sut.GetBySKUAsync(lineItemIds, properties, sku);

            //Assert
            Assert.That(lineItem.LineItems.Contains(result));
        }

        [Test, CustomAutoData]
        public async Task GetBySKUAsync_returns_lineitem_with_valid_id([Frozen] IHttpRestClient httpRestClient, IHubSpotLineItemClient sut, IReadOnlyList<long> lineItemIds, IFixture fixture,
            IReadOnlyList<IProperty> properties, string sku)
        {
            //Arrange
            var lineItem = fixture.Build<LineItem>().With(x => x.Id, lineItemIds.FirstOrDefault()).Create();
            var lineItemResult = fixture.Build<LineItemResult>().With(x => x.LineItems, new List<LineItem> { lineItem }).Create();
            Mock.Get(httpRestClient)
                .Setup(p => p.SendAsync<object, LineItemResult>(HttpMethod.Post, $"/crm/v3/objects/line_items/search", It.IsAny<object>(), null)).Returns(Task.FromResult(lineItemResult));

            //Act
            var result = await sut.GetBySKUAsync(lineItemIds, properties, sku);

            //Assert
            Assert.That(lineItemIds.Contains(result.Id));
        }

        #endregion
    }
}
