using Microsoft.Extensions.Configuration;
using RestSharp;
using RichardSzalay.MockHttp;
using SmartShareClient;
using SmartShareClient.Model;
using System.Net;

namespace SmartShareClientTests
{
    public class SmartShareTests
    {
        private readonly Dictionary<string, string> appSettingsStub = new()
        {
            {"SmartShare:Endpoint", "http://fakeendpoint"},
            {"SmartShare:ClientId", "fakeclientid"},
            {"SmartShare:ClientKey", "fakeclientkey"},
            {"SmartShare:User", "fakeuser"},
            {"SmartShare:Password", "fakepass"},
        };

        private readonly MockHttpMessageHandler mockHttp = new();

        private readonly IConfiguration configuration;

        public SmartShareTests()
        {
            this.configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();
        }

        [Fact]
        public async Task SmartShareClient_GenerateTokenAsync_EnsureCorrectDeserialization()
        {
            // Arrange
            var fakeTokenResponse = new GenerateTokenResponse
            {
                CdUsuario = 1,
                TokenUsuario = Guid.NewGuid().ToString()
            };

            this.mockHttp
                .When("*")
                .Respond(
                    HttpStatusCode.OK,
                    "application/json",
                    @$"{{
                            ""cdUsuario"": {fakeTokenResponse.CdUsuario},
                            ""tokenUsuario"": ""{fakeTokenResponse.TokenUsuario}""
                       }}");

            var options = new RestClientOptions("http://fakeendpoint")
            {
                ConfigureMessageHandler = handler => mockHttp
            };

            var restClient = new RestClient(options);

            var smartShareClient = new SmartShare(configuration, restClient);

            // Act
            var result = await smartShareClient.GenerateTokenAsync();

            // Assert
            Assert.Equal(result.CdUsuario, fakeTokenResponse.CdUsuario);
            Assert.Equal(result.TokenUsuario, fakeTokenResponse.TokenUsuario);
        }
    }
}