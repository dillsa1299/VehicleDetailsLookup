using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using VehicleDetailsLookup.Models.ApiResponses.GoogleImage;
using VehicleDetailsLookup.Services.Api.GoogleImage;
using Xunit;

namespace VehicleDetailsLookup.Tests.Services.Api.GoogleImage
{
    public class GoogleImageServiceTests
    {
        private static IConfiguration GetConfig(string key = "test-key", string cx = "test-cx")
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                {"APIs:Google:Key", key},
                {"APIs:Google:Cx", cx}
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        private static HttpClient GetMockHttpClient(HttpResponseMessage response)
        {
            var handlerMock = new Moq.Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            return new HttpClient(handlerMock.Object);
        }

        [Theory]
        [InlineData("APIs:Google:Key")]
        [InlineData("APIs:Google:Cx")]
        public void Constructor_Throws_OnMissingConfig(string missingKey)
        {
            // Arrange
            var httpClient = new HttpClient();

            var settings = new Dictionary<string, string?>
            {
                {"APIs:Google:Key", "test-key"},
                {"APIs:Google:Cx", "test-cx"}
            };
            settings.Remove(missingKey);

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => new GoogleImageService(httpClient, config));
        }

        [Fact]
        public async Task GetGoogleImageResponseAsync_ReturnsModel_OnSuccess()
        {
            // Arrange
            var expected = new GoogleImageResponseModel { Kind = "customsearch#search" };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expected)
            };
            var httpClient = GetMockHttpClient(response);
            var config = GetConfig();
            var service = new GoogleImageService(httpClient, config);

            // Act
            var result = await service.GetGoogleImageResponseAsync("car");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("customsearch#search", result!.Kind);
        }

        [Fact]
        public async Task GetGoogleImageResponseAsync_ReturnsNull_OnFailure()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var httpClient = GetMockHttpClient(response);
            var config = GetConfig();
            var service = new GoogleImageService(httpClient, config);

            // Act
            var result = await service.GetGoogleImageResponseAsync("car");

            // Assert
            Assert.Null(result);
        }
    }
}
