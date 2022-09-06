using AutoFixture.NUnit3;
using HubSpot.LineItems;
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
        public async Task GetByIdAsync_returns_lineitem_generated_by_connector([Frozen] IHubSpotLineItemConnector lineItemConnector, long lineItemId, LineItem expected)
        {
            //Arrange
            Mock.Get(lineItemConnector).Setup(x => x.GetAsync<LineItem>(It.IsAny<ILineItemSelector>())).ReturnsAsync(expected);

            //Assert
            var result = await LineItemSelectorExtensions.GetByIdAsync<LineItem>(lineItemConnector, lineItemId);

            //Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test, CustomAutoData]
        public async Task GetByIdAsync_calls_connector_GetAsync_method([Frozen] IHubSpotLineItemConnector lineItemConnector, long lineItemId, LineItem expected)
        {
            //Arrange
            Mock.Get(lineItemConnector).Setup(x => x.GetAsync<LineItem>(It.IsAny<ILineItemSelector>())).ReturnsAsync(expected);

            //Assert
            var result = await LineItemSelectorExtensions.GetByIdAsync<LineItem>(lineItemConnector, lineItemId);

            //Assert
            Mock.Get(lineItemConnector).Verify(x => x.GetAsync<LineItem>(It.IsAny<ILineItemSelector>()), Times.Once);
        }
        #endregion
        #region GetBySKUAsync
        [Test, CustomAutoData]
        public async Task GetBySKUAsync_returns_lineitem_generated_by_connector([Frozen] IHubSpotLineItemConnector lineItemConnector, IReadOnlyList<long> lineItemIds, string sku, LineItem expected)
        {
            //Arrange
            Mock.Get(lineItemConnector).Setup(x => x.GetBySKUAsync<LineItem>(It.IsAny<ILineItemSelector>(), sku)).ReturnsAsync(expected);

            //Assert
            var result = await LineItemSelectorExtensions.GetBySKUAsync<LineItem>(lineItemConnector, lineItemIds, sku);

            //Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test, CustomAutoData]
        public async Task GetBySKUAsync_calls_connector_GetBySKUAsync_method([Frozen] IHubSpotLineItemConnector lineItemConnector, IReadOnlyList<long> lineItemIds, string sku, LineItem expected)
        {
            //Arrange
            Mock.Get(lineItemConnector).Setup(x => x.GetBySKUAsync<LineItem>(It.IsAny<ILineItemSelector>(), sku)).ReturnsAsync(expected);

            //Assert
            var result = await LineItemSelectorExtensions.GetBySKUAsync<LineItem>(lineItemConnector, lineItemIds, sku);

            //Assert
            Mock.Get(lineItemConnector).Verify(x => x.GetBySKUAsync<LineItem>(It.IsAny<ILineItemSelector>(), sku), Times.Once);
        }
        #endregion
    }
}
