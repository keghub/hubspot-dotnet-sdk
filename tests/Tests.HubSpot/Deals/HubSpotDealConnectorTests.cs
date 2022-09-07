using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Deals;
using HubSpot.Model.Deals;
using HubSpot.Model.LineItems;
using HubSpot.Model.Pipelines;
using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Deals
{
    [TestFixture]
    public class HubSpotDealConnectorTests
    {
        [Test, CustomAutoData]
        public void Constructor_is_guarded_against_null_parameters(GuardClauseAssertion guardClauseAssertion)
        {
            //Arrange

            //Act

            //Assert
            guardClauseAssertion.Verify(typeof(HubSpotDealConnector).GetConstructors());
        }

        [Test, CustomAutoData]
        public void GetAsync_is_guarded_against_null_selector(HubSpotDealConnector sut)
        {
            //Arrange

            //Act

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.GetAsync<HubSpot.Deals.Deal>(null));
        }

        [Test, CustomAutoData]
        public async Task GetAsync_invokes_selector_from_parameters_to_retrieve_deal([Frozen] IHubSpotClient hubSpotClient, [Frozen] IDealSelector dealSelector, HubSpotDealConnector sut)
        {
            //Arrange

            //Act
            await sut.GetAsync<HubSpot.Deals.Deal>(dealSelector);

            //Assert
            Mock.Get(dealSelector).Verify(x => x.GetDeal(hubSpotClient), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task GetAsync_invokes_deal_type_manager_to_convert_selector_result([Frozen] IDealTypeManager dealTypeManager, [Frozen] IHubSpotClient hubSpotClient, [Frozen] IDealSelector dealSelector, HubSpotDealConnector sut,HubSpot.Model.Deals.Deal deal)
        {
            //Arrange
            Mock.Get(dealSelector).Setup(x => x.GetDeal(hubSpotClient)).Returns(Task.FromResult(deal));

            //Act
            await sut.GetAsync<HubSpot.Deals.Deal>(dealSelector);

            //Assert
            Mock.Get(dealTypeManager).Verify(x => x.ConvertTo<HubSpot.Deals.Deal>(deal), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task GetAsync_returns_converted_deal([Frozen] IDealTypeManager dealTypeManager, [Frozen] IHubSpotClient hubSpotClient, [Frozen] IDealSelector dealSelector, HubSpot.Deals.Deal convertedDeal, HubSpotDealConnector sut, IFixture fixture) 
        {
            //Arrange
            Mock.Get(dealTypeManager).Setup(x => x.ConvertTo<HubSpot.Deals.Deal>(It.IsAny<HubSpot.Model.Deals.Deal>())).Returns(convertedDeal);

            //Act
            var result = await sut.GetAsync<HubSpot.Deals.Deal>(dealSelector);

            //Assert
            Assert.That(result, Is.EqualTo(convertedDeal));
        }

        [Test, CustomAutoData]
        public async Task GetAsync_returns_null_if_deal_retrieving_throws_NotFoundException([Frozen] IHubSpotClient hubSpotClient, [Frozen] IDealSelector dealSelector, HubSpotDealConnector sut, NotFoundException notFoundException)
        {
            //Arrange
            Mock.Get(dealSelector).Setup(x => x.GetDeal(hubSpotClient)).Throws(notFoundException);

            //Act
            var result = await sut.GetAsync<HubSpot.Deals.Deal>(dealSelector);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test, CustomAutoData]
        public async Task GetDealLineItemsAsync_returns_null_if_deal_client_returns_null([Frozen] IHubSpotClient hubSpotClient, long dealId, HubSpotDealConnector sut)
        {
            //Arrange
            Mock.Get(hubSpotClient).Setup(x => x.Deals.GetLineItemAssociationsAsync(dealId)).Returns(Task.FromResult((LineItemAssociationList)null));

            //Act
            var result = await sut.GetDealLineItemsAsync(dealId);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test, CustomAutoData]
        public async Task GetDealLineItemsAsync_returns_same_amount_of_records_as_deal_client([Frozen] IHubSpotClient hubSpotClient, long dealId, HubSpotDealConnector sut, IFixture fixture)
        {
            //Arrange
            var lineItemAssociations = fixture.Build<LineItemAssociation>().CreateMany().ToList().AsReadOnly();
            var lineItemAssociationList = fixture.Build<LineItemAssociationList>().With(x => x.LineItemAssociations, lineItemAssociations).Create();
            Mock.Get(hubSpotClient).Setup(x => x.Deals.GetLineItemAssociationsAsync(dealId)).Returns(Task.FromResult(lineItemAssociationList));

            //Act
            var result = await sut.GetDealLineItemsAsync(dealId);

            //Assert
            Assert.That(result.Count, Is.EqualTo(lineItemAssociations.Count));
        }

        [Test, CustomAutoData]
        public async Task GetDealLineItemsAsync_returns_valid_line_items_ids_as_deal_client([Frozen] IHubSpotClient hubSpotClient, long dealId, HubSpotDealConnector sut, IFixture fixture)
        {
            //Arrange 
            var lineItemAssociations = fixture.Build<LineItemAssociation>().CreateMany().ToList().AsReadOnly();
            var lineItemAssociationList = fixture.Build<LineItemAssociationList>().With(x => x.LineItemAssociations, lineItemAssociations).Create();
            Mock.Get(hubSpotClient).Setup(x => x.Deals.GetLineItemAssociationsAsync(dealId)).Returns(Task.FromResult(lineItemAssociationList));

            var expected = lineItemAssociations.Select(x => x.Id).ToList().AsReadOnly();

            //Act
            var result = await sut.GetDealLineItemsAsync(dealId);

            //Assert
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }
}
