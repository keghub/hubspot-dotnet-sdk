using AutoFixture.NUnit3;
using HubSpot.LineItems;
using HubSpot.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Internal
{
    [TestFixture]
    public class LineItemSelectorExtensionsTests
    {
        #region GetByIdAsync
        [Test, CustomAutoData]
        public async Task GetByIdAsync_returns_lineitem_generated_by_connector([Frozen] IHubSpotLineItemConnector lineItemConnector, long lineItemId, LineItem expected, Property[] properties)
        {
            //Arrange
            Mock.Get(lineItemConnector).Setup(x => x.GetAsync<LineItem>(It.IsAny<ILineItemSelector>(), properties)).ReturnsAsync(expected);

            //Assert
            var result = await LineItemSelectorExtensions.GetByIdAsync<LineItem>(lineItemConnector, lineItemId, properties);

            //Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test, CustomAutoData]
        public async Task GetByIdAsync_calls_connector_GetAsync_method([Frozen] IHubSpotLineItemConnector lineItemConnector, long lineItemId, LineItem expected, Property[] properties)
        {
            //Arrange
            Mock.Get(lineItemConnector).Setup(x => x.GetAsync<LineItem>(It.IsAny<ILineItemSelector>(), properties)).ReturnsAsync(expected);

            //Assert
            var result = await LineItemSelectorExtensions.GetByIdAsync<LineItem>(lineItemConnector, lineItemId, properties);

            //Assert
            Mock.Get(lineItemConnector).Verify(x => x.GetAsync<LineItem>(It.IsAny<ILineItemSelector>(), properties), Times.Once);
        }
        #endregion
        #region GetBySKUAsync
        [Test, CustomAutoData]
        public async Task GetBySKUAsync_returns_lineitem_generated_by_connector([Frozen] IHubSpotLineItemConnector lineItemConnector, IReadOnlyList<long> lineItemIds, string sku, LineItem expected, Property[] properties)
        {
            //Arrange
            Mock.Get(lineItemConnector).Setup(x => x.GetBySKUAsync<LineItem>(It.IsAny<ILineItemSelector>(), sku, properties)).ReturnsAsync(expected);

            //Assert
            var result = await LineItemSelectorExtensions.GetBySKUAsync<LineItem>(lineItemConnector, lineItemIds, sku, properties);

            //Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test, CustomAutoData]
        public async Task GetBySKUAsync_calls_connector_GetBySKUAsync_method([Frozen] IHubSpotLineItemConnector lineItemConnector, IReadOnlyList<long> lineItemIds, string sku, LineItem expected, Property[] properties)
        {
            //Arrange
            Mock.Get(lineItemConnector).Setup(x => x.GetBySKUAsync<LineItem>(It.IsAny<ILineItemSelector>(), sku, properties)).ReturnsAsync(expected);

            //Assert
            var result = await LineItemSelectorExtensions.GetBySKUAsync<LineItem>(lineItemConnector, lineItemIds, sku, properties);

            //Assert
            Mock.Get(lineItemConnector).Verify(x => x.GetBySKUAsync<LineItem>(It.IsAny<ILineItemSelector>(), sku, properties), Times.Once);
        }
        #endregion
    }
}
