using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using VehicleDetailsLookup.Models.ApiResponses.Ves;
using VehicleDetailsLookup.Services.Api.Ves;

namespace VehicleDetailsLookup.Tests.Services.Api.Ves
{
    public class VesServiceTests
    {
        private static IConfiguration GetConfig(string apiKey = "key", string url = "https://ves.api")
        {
            var settings = new Dictionary<string, string?>
            {
                {"APIs:VES:Key", apiKey},
                {"APIs:VES:URL", url}
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
        }

        private static HttpClient GetMockHttpClient(params HttpResponseMessage[] responses)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var queue = new Queue<Func<HttpResponseMessage>>();

            foreach (var response in responses)
            {
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
        [InlineData("APIs:VES:Key")]
        [InlineData("APIs:VES:URL")]
        public void Constructor_Throws_OnMissingConfig(string missingKey)
        {
            // Arrange
            var httpClient = new HttpClient();

            var settings = new Dictionary<string, string?>
            {
                {"APIs:VES:Key", "key"},
                {"APIs:VES:URL", "url"}
            };
            settings.Remove(missingKey);

            var config = new ConfigurationBuilder().AddInMemoryCollection(settings!).Build();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => new VesService(httpClient, config));
        }

        [Fact]
        public async Task GetVesResponseAsync_ReturnsModel_OnSuccess()
        {
            // Arrange
            var vesModel = new VesResponseModel { RegistrationNumber = "ABC123" };
            var vesJson = JsonSerializer.Serialize(vesModel);
            var vesResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(vesJson)
            };

            var httpClient = GetMockHttpClient(vesResponse);
            var config = GetConfig();
            var service = new VesService(httpClient, config);

            // Act
            var result = await service.GetVesResponseAsync("ABC123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ABC123", result.RegistrationNumber);
        }

        [Fact]
        public async Task GetVesResponseAsync_ReturnsNull_OnApiFailure()
        {
            // Arrange
            var vesResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);

            var httpClient = GetMockHttpClient(vesResponse);
            var config = GetConfig();
            var service = new VesService(httpClient, config);

            // Act
            var result = await service.GetVesResponseAsync("ABC123");

            // Assert
            Assert.Null(result);
        }
    }
}
