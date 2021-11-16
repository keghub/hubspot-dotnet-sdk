using AutoFixture;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using HubSpot;
using HubSpot.Model.Pipelines;
using Kralizek.Extensions.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Pipelines
{
    [TestFixture]
    public class HubSpotPipelineClientTests
    {
        [Test, CustomAutoData]
        public void Constructor_is_guarded_against_null_parameters(GuardClauseAssertion guardClauseAssertion)
        {
            //Arrange

            //Act

            //Assert
            guardClauseAssertion.Verify(typeof(HttpHubSpotClient).GetConstructors());
        }

        [Test, CustomAutoData]
        public void GetByGuidAsync_throws_NotFoundException_if_request_returns_NotFound_status([Frozen] IHttpRestClient httpRestClient, IHubSpotPipelineClient sut, string guid)
        {
            //Arrange
            Mock.Get(httpRestClient)
                .Setup(p => p.SendAsync<Pipeline>(HttpMethod.Get, $"/deals/v1/pipelines/{guid}", null))
                .Throws(new HttpException("Not Found", HttpStatusCode.NotFound));

            //Act

            //Assert
            Assert.That(() => sut.GetByGuidAsync(guid), Throws.InstanceOf<NotFoundException>());
        }

        [Test, CustomAutoData]
        public void GetByGuidAsync_invokes_http_client_get_method_once_with_guid([Frozen] IHttpRestClient httpRestClient, IHubSpotPipelineClient sut, string guid)
        {
            //Arrange

            //Act
            sut.GetByGuidAsync(guid);

            //Assert
            Mock.Get(httpRestClient).Verify(p => p.SendAsync<Pipeline>(HttpMethod.Get, $"/deals/v1/pipelines/{guid}", null), Times.Once);
        }

        [Test, CustomAutoData]
        public async Task GetByGuidAsync_returns_null_if_guid_is_empty([Frozen] IHttpRestClient httpRestClient, IHubSpotPipelineClient sut)
        {
            //Arrange
            Mock.Get(httpRestClient)
                .Setup(p => p.SendAsync<Pipeline>(HttpMethod.Get, $"/deals/v1/pipelines/{string.Empty}", null)).Returns(Task.FromResult((Pipeline)null));

            //Act
            var result = await sut.GetByGuidAsync(string.Empty);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test, CustomAutoData]
        public async Task GetByGuidAsync_returns_pipeline_retrieved_by_http_client_request([Frozen] IHttpRestClient httpRestClient,[Frozen] Pipeline pipeline, IHubSpotPipelineClient sut, string guid)
        {
            //Arrange
            Mock.Get(httpRestClient)
                .Setup(p => p.SendAsync<Pipeline>(HttpMethod.Get, $"/deals/v1/pipelines/{guid}", null)).Returns(Task.FromResult(pipeline));

            //Act
            var result = await sut.GetByGuidAsync(guid);

            //Assert
            Assert.That(result, Is.EqualTo(pipeline));
        }

        [Test, CustomAutoData]
        public async Task GetByGuidAsync_returns_pipeline_with_valid_guid([Frozen] IHttpRestClient httpRestClient, IHubSpotPipelineClient sut, string guid, IFixture fixture)
        {
            //Arrange
            var pipeline = fixture.Build<Pipeline>().With(x => x.Guid, guid).Create();
            Mock.Get(httpRestClient)
                .Setup(p => p.SendAsync<Pipeline>(HttpMethod.Get, $"/deals/v1/pipelines/{guid}", null)).Returns(Task.FromResult(pipeline));

            //Act
            var result = await sut.GetByGuidAsync(guid);

            //Assert
            Assert.That(result.Guid, Is.EqualTo(guid));
        }
    }
}
