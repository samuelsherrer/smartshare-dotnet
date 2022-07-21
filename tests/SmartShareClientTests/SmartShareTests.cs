using FizzWare.NBuilder;
using Microsoft.Extensions.Configuration;
using Moq;
using RestSharp;
using SmartShareClient;
using SmartShareClient.Model;
using System.Net;

namespace SmartShareClientTests
{
    public class SmartShareTests
    {
        private SmartShare smartShareClient;
        protected readonly Mock<IRestClient> mockRestClient;

        private Dictionary<string, string> appSettingsStub = new Dictionary<string, string> {
            {"SmartShare:Endpoint", "http://fakeendpoint"},
            {"SmartShare:ClientId", "fakeclientid"},
            {"SmartShare:ClientKey", "fakeclientkey"},
            {"SmartShare:User", "fakeuser"},
            {"SmartShare:Password", "fakepass"},
        };

        public SmartShareTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            this.mockRestClient = new Mock<IRestClient>();
            this.smartShareClient = new SmartShare(configuration, mockRestClient.Object);
        }

        [Fact]
        public async Task SmartShareClient_GenerateTokenAsync_EnsureCorrectDeserialization()
        {
            // Arrange
            var fakeTokenResponse = Builder<GenerateTokenResponse>.CreateNew().Build();

            this.mockRestClient
                .Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), CancellationToken.None))
                .ReturnsAsync(new RestResponse() 
                { 
                    StatusCode = HttpStatusCode.OK,
                    ResponseStatus = ResponseStatus.Completed,
                    Content = @$"{{
                                    ""cdUsuario"": {fakeTokenResponse.CdUsuario},
                                    ""tokenUsuario"": ""{fakeTokenResponse.TokenUsuario}""
                                  }}"
                });

            // Act
            var result = await smartShareClient.GenerateTokenAsync();

            // Assert
            Assert.Equal(result.CdUsuario, fakeTokenResponse.CdUsuario);
            Assert.Equal(result.TokenUsuario, fakeTokenResponse.TokenUsuario);
        }
    }
}