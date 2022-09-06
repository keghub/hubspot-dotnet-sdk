using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Internal;
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
    public class HubSpotLineItemConnectorTests
    {
        [Test, CustomAutoData]
        public void Constructor_is_guarded_against_null_parameters(GuardClauseAssertion guardClauseAssertion)
        {
            //Arrange

            //Act

            //Assert
            guardClauseAssertion.Verify(typeof(HubSpotLineItemConnector).GetConstructors());
        }

        #region GetAsync
        [Test, CustomAutoData]
        public void GetAsync_is_guarded_against_null_parameters(HubSpotLineItemConnector sut)
        {
            //Arrange

            //Act

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetAsync<LineItem>(null, null));
        }

        [Test, CustomAutoData]
        public async Task GetAsync_invoke_selector_from_parameters_to_Retrieve_lineItem([Frozen] IHubSpotClient hubSpotClient, [Frozen] ILineItemSelector lineItemSelector, Property[] properties, HubSpotLineItemConnector sut)
        {
            //Arrange

            //Act
            await sut.GetAsync<LineItem>(lineItemSelector, properties);

            //Assert
            Mock.Get(lineItemSelector).Verify(x => x.GetLineItemAsync(hubSpotClient, It.IsAny<IReadOnlyList<IProperty>>()), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task GetAsync_invoke_type_manager_to_convert_selector_result([Frozen] ILineItemTypeManager lineItemTypeManager, [Frozen] ILineItemSelector lineItemSelector, [Frozen] IHubSpotClient hubspotClient, Property[] properties,
            HubSpotLineItemConnector sut, IFixture fixture)
        {
            //Arrange
            var lineItem = fixture.Build<HubSpot.Model.LineItems.LineItem>().Without(x => x.Properties).Create();
            Mock.Get(lineItemSelector).Setup(x => x.GetLineItemAsync(hubspotClient, It.IsAny<IReadOnlyList<IProperty>>())).Returns(Task.FromResult(lineItem));

            //Act
            await sut.GetAsync<LineItem>(lineItemSelector, properties);

            //Assert
            Mock.Get(lineItemTypeManager).Verify(x => x.ConvertTo<LineItem>(lineItem), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task GetAsync_returns_converted_line_item([Frozen] LineItem lineItem, [Frozen] ILineItemTypeManager lineItemTypeManager, [Frozen] ILineItemSelector lineItemSelector, Property[] properties,
            HubSpotLineItemConnector sut)
        {
            //Arrange
            Mock.Get(lineItemTypeManager).Setup(x => x.ConvertTo<LineItem>(It.IsAny<HubSpot.Model.LineItems.LineItem>())).Returns(lineItem);

            //Act
            var result = await sut.GetAsync<LineItem>(lineItemSelector, properties);

            //Assert
            Assert.That(result, Is.EqualTo(lineItem));
        }

        [Test, CustomAutoData]
        public async Task GetAsync_returns_null_if_lineitem_retrieving_throws_NotFoundException([Frozen] IHubSpotClient hubSpotClient, [Frozen] ILineItemSelector lineItemSelector, Property[] properties, HubSpotLineItemConnector sut, NotFoundException notFoundException)
        {
            //Arrange
            Mock.Get(lineItemSelector).Setup(x => x.GetLineItemAsync(hubSpotClient, It.IsAny<IReadOnlyList<IProperty>>())).Throws(notFoundException);

            //Act
            var result = await sut.GetAsync<LineItem>(lineItemSelector, properties);

            //Assert
            Assert.That(result, Is.Null);
        }
        #endregion

        #region GetBySKUAsync
        [Test, CustomAutoData]
        public void GetBySKUAsync_is_guarded_against_null_parameters(HubSpotLineItemConnector sut)
        {
            //Arrange

            //Act

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetBySKUAsync<LineItem>(null, string.Empty, null));
        }

        [Test, CustomAutoData]
        public async Task GetBySKUAsync_invoke_selector_from_parameters_to_Retrieve_lineItem([Frozen] IHubSpotClient hubSpotClient, [Frozen] ILineItemSelector lineItemSelector, Property[] properties, HubSpotLineItemConnector sut,
            string sku)
        {
            //Arrange

            //Act
            await sut.GetBySKUAsync<LineItem>(lineItemSelector, sku, properties);

            //Assert
            Mock.Get(lineItemSelector).Verify(x => x.GetLineItemBySKUAsync(hubSpotClient, It.IsAny<IReadOnlyList<IProperty>>(), sku), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task GetBySKUAsync_invoke_type_manager_to_convert_selector_result([Frozen] ILineItemTypeManager lineItemTypeManager, [Frozen] ILineItemSelector lineItemSelector, [Frozen] IHubSpotClient hubspotClient, 
            Property[] properties, HubSpotLineItemConnector sut, IFixture fixture, string sku)
        {
            //Arrange
            var lineItem = fixture.Build<HubSpot.Model.LineItems.LineItem>().Without(x => x.Properties).Create();
            Mock.Get(lineItemSelector).Setup(x => x.GetLineItemBySKUAsync(hubspotClient, It.IsAny<IReadOnlyList<IProperty>>(), sku)).Returns(Task.FromResult(lineItem));

            //Act
            await sut.GetBySKUAsync<LineItem>(lineItemSelector, sku, properties);

            //Assert
            Mock.Get(lineItemTypeManager).Verify(x => x.ConvertTo<LineItem>(lineItem), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task GetBySKUAsync_returns_converted_line_item([Frozen] LineItem lineItem, [Frozen] ILineItemTypeManager lineItemTypeManager, [Frozen] ILineItemSelector lineItemSelector, Property[] properties, 
            HubSpotLineItemConnector sut, string sku)
        {
            //Arrange
            Mock.Get(lineItemTypeManager).Setup(x => x.ConvertTo<LineItem>(It.IsAny<HubSpot.Model.LineItems.LineItem>())).Returns(lineItem);

            //Act
            var result = await sut.GetBySKUAsync<LineItem>(lineItemSelector, sku, properties);

            //Assert
            Assert.That(result, Is.EqualTo(lineItem));
        }

        [Test, CustomAutoData]
        public async Task GetBySKUAsync_returns_null_if_lineitem_retrieving_throws_NotFoundException([Frozen] IHubSpotClient hubSpotClient, [Frozen] ILineItemSelector lineItemSelector, 
            Property[] properties, HubSpotLineItemConnector sut, NotFoundException notFoundException, string sku)
        {
            //Arrange
            Mock.Get(lineItemSelector).Setup(x => x.GetLineItemBySKUAsync(hubSpotClient, It.IsAny<IReadOnlyList<IProperty>>(), sku)).Throws(notFoundException);

            //Act
            var result = await sut.GetBySKUAsync<LineItem>(lineItemSelector, sku, properties);

            //Assert
            Assert.That(result, Is.Null);
        }
        #endregion
    }
}
