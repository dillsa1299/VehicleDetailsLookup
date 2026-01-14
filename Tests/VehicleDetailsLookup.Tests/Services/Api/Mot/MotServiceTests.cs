using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using VehicleDetailsLookup.Models.ApiResponses.Mot;
using VehicleDetailsLookup.Services.Api.Mot;

namespace VehicleDetailsLookup.Tests.Services.Api.Mot
{
    public class MotServiceTests
    {
        private static IConfiguration GetConfig(
            string url = "https://mot.api", string clientId = "id", string clientSecret = "secret",
            string apiKey = "key", string scopeUrl = "scope", string tokenUrl = "https://token.api")
        {
            var settings = new Dictionary<string, string?>
            {
                {"APIs:MOT:URL", url},
                {"APIs:MOT:ClientId", clientId},
                {"APIs:MOT:ClientSecret", clientSecret},
                {"APIs:MOT:Key", apiKey},
                {"APIs:MOT:ScopeUrl", scopeUrl},
                {"APIs:MOT:TokenUrl", tokenUrl}
            };
            return new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();
        }

        private static HttpClient GetMockHttpClient(params HttpResponseMessage[] responses)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var queue = new Queue<Func<HttpResponseMessage>>();

            foreach (var response in responses)
            {
                // Enqueue a factory to create a new response with new content each time
                queue.Enqueue(() => new HttpResponseMessage(response.StatusCode)
                {
                    Content = response.Content == null ? null : new StringContent(response.Content.ReadAsStringAsync().Result)
                });
            }

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => queue.Dequeue().Invoke());

            return new HttpClient(handlerMock.Object);
        }

        [Theory]
        [InlineData("APIs:MOT:URL")]
        [InlineData("APIs:MOT:ClientId")]
        [InlineData("APIs:MOT:ClientSecret")]
        [InlineData("APIs:MOT:Key")]
        [InlineData("APIs:MOT:ScopeUrl")]
        [InlineData("APIs:MOT:TokenUrl")]
        public void Constructor_Throws_OnMissingConfig(string missingKey)
        {
            // Arrange
            var httpClient = new HttpClient();

            var settings = new Dictionary<string, string?>
            {
                {"APIs:MOT:URL", "url"},
                {"APIs:MOT:ClientId", "id"},
                {"APIs:MOT:ClientSecret", "secret"},
                {"APIs:MOT:Key", "key"},
                {"APIs:MOT:ScopeUrl", "scope"},
                {"APIs:MOT:TokenUrl", "token"}
            };
            settings.Remove(missingKey);

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(settings!)
                .Build();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => new MotService(httpClient, config));
        }

        [Fact]
        public async Task GetMotResponseAsync_ReturnsModel_OnSuccess()
        {
            // Arrange
            var tokenJson = "{\"token_type\":\"Bearer\",\"expires_in\":3600,\"access_token\":\"abc123\"}";
            var motModel = new MotResponseModel { Registration = "ABC123" };
            var motJson = JsonSerializer.Serialize(motModel);

            var tokenResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(tokenJson)
            };

            var motResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(motJson)
            };

            var httpClient = GetMockHttpClient(tokenResponse, motResponse);
            var config = GetConfig();
            var service = new MotService(httpClient, config);

            // Act
            var result = await service.GetMotResponseAsync("ABC123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ABC123", result!.Registration);
        }

        [Fact]
        public async Task GetMotResponseAsync_ReturnsNull_OnTokenFailure()
        {
            // Arrange
            var tokenResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var httpClient = GetMockHttpClient(tokenResponse);
            var config = GetConfig();
            var service = new MotService(httpClient, config);

            // Act
            var result = await service.GetMotResponseAsync("ABC123");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetMotResponseAsync_ReturnsNull_OnMotApiFailure()
        {
            // Arrange
            var tokenJson = "{\"token_type\":\"Bearer\",\"expires_in\":3600,\"access_token\":\"abc123\"}";

            var tokenResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(tokenJson)
            };

            var motResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var httpClient = GetMockHttpClient(tokenResponse, motResponse);
            var config = GetConfig();
            var service = new MotService(httpClient, config);

            // Act
            var result = await service.GetMotResponseAsync("ABC123");

            // Assert
            Assert.Null(result);
        }
    }
}
