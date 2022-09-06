using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.LineItems;
using HubSpot.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests.LineItems
{
    [TestFixture]
    public class LineItemSelectorTests
    {
        #region GetLineItemAsync
        [Test, CustomAutoData]
        public async Task GetLineItemAsync_returns_line_item_generated_by_client([Frozen]IHubSpotClient client, [Frozen]IReadOnlyList<IProperty> properties, HubSpot.Model.LineItems.LineItem lineItem, LineItemSelector sut)
        {
            //Arrange
            Mock.Get(client).Setup(x => x.LineItems.GetAsync(It.IsAny<long>(), It.IsAny<IReadOnlyList<IProperty>>())).Returns(Task.FromResult(lineItem));

            //Act
            var result = await sut.GetLineItemAsync(client, properties);

            //Assert
            Assert.That(result, Is.EqualTo(lineItem));
        }

        [Test, CustomAutoData]
        public async Task GetLineItemAsync_returns_null_if_client_returns_null([Frozen] IHubSpotClient client, [Frozen] IReadOnlyList<IProperty> properties, HubSpot.Model.LineItems.LineItem lineItem, LineItemSelector sut)
        {
            //Arrange
            Mock.Get(client).Setup(x => x.LineItems.GetAsync(It.IsAny<long>(), It.IsAny<IReadOnlyList<IProperty>>())).Returns(Task.FromResult((HubSpot.Model.LineItems.LineItem)null));

            //Act
            var result = await sut.GetLineItemAsync(client, properties);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test, CustomAutoData]
        public async Task GetLineItemAsync_calls_client_with_valid_lineitemId([Frozen] IHubSpotClient client, [Frozen] IReadOnlyList<IProperty> properties, long lineItemId)
        {
            //Arrange
            var sut = new LineItemSelector(lineItemId);

            //Act
            var result = await sut.GetLineItemAsync(client, properties);

            //Assert
            Mock.Get(client).Verify(x => x.LineItems.GetAsync(lineItemId, properties), Times.Once);
        }
        #endregion
        #region GetLineItemBySKUAsync
        [Test, CustomAutoData]
        public async Task GetLineItemBySKUAsync_returns_line_item_generated_by_client([Frozen] IHubSpotClient client, [Frozen] IReadOnlyList<IProperty> properties, HubSpot.Model.LineItems.LineItem lineItem, string sku, LineItemSelector sut)
        {
            //Arrange
            Mock.Get(client).Setup(x => x.LineItems.GetBySKUAsync(It.IsAny<IReadOnlyList<long>>(), It.IsAny<IReadOnlyList<IProperty>>(), sku)).Returns(Task.FromResult(lineItem));

            //Act
            var result = await sut.GetLineItemBySKUAsync(client, properties, sku);

            //Assert
            Assert.That(result, Is.EqualTo(lineItem));
        }

        [Test, CustomAutoData]
        public async Task GetLineItemBySKUAsync_returns_null_if_client_returns_null([Frozen] IHubSpotClient client, [Frozen] IReadOnlyList<IProperty> properties, string sku, LineItemSelector sut)
        {
            //Arrange
            Mock.Get(client).Setup(x => x.LineItems.GetBySKUAsync(It.IsAny<IReadOnlyList<long>>(), It.IsAny<IReadOnlyList<IProperty>>(), sku)).Returns(Task.FromResult((HubSpot.Model.LineItems.LineItem)null));

            //Act
            var result = await sut.GetLineItemBySKUAsync(client, properties, sku);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test, CustomAutoData]
        public async Task GetLineItemBySKUAsync_calls_client_with_valid_lineitemIds([Frozen] IHubSpotClient client, [Frozen] IReadOnlyList<IProperty> properties, HubSpot.Model.LineItems.LineItem lineItem, string sku, IReadOnlyList<long> lineItemIds)
        {
            //Arrange
            var sut = new LineItemSelector(lineItemIds);

            //Act
            var result = await sut.GetLineItemBySKUAsync(client, properties, sku);

            //Assert
            Mock.Get(client).Verify(x => x.LineItems.GetBySKUAsync(lineItemIds, properties, sku), Times.Once);
        }
        #endregion
    }
}
