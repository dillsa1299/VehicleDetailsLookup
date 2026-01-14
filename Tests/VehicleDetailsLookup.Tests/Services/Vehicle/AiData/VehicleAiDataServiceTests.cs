using Moq;
using System;
using System.Threading.Tasks;
using VehicleDetailsLookup.Models.Database.AiData;
using VehicleDetailsLookup.Shared.Models.Ai;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Requests;
using Xunit;

namespace VehicleDetailsLookup.Tests.Services.Vehicle.AiData
{
    public class VehicleAiDataServiceTests : IClassFixture<VehicleAiDataServiceFixture>
    {
        private readonly VehicleAiDataServiceFixture _fixture;

        public VehicleAiDataServiceTests(VehicleAiDataServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetVehicleAiDataAsync_ReturnsMappedAiData_WhenDbDataIsRecent()
        {
            // Arrange
            var registration = "ABC123";
            var searchType = AiType.Overview;
            var metaData = "meta";
            var dbAiData = new AiDataDbModel
            {
                RegistrationNumber = registration,
                Type = searchType,
                MetaData = metaData,
                GeneratedText = "db-response",
                Updated = DateTime.UtcNow,
                DataHash = "hash"
            };
            var mapped = new AiDataModel { Type = searchType, Content = "db-response", MetaData = metaData };

            _fixture.MockGetAiDataAsync(registration, searchType, metaData, dbAiData);
            _fixture.MockDatabaseMapper_MapAiData(dbAiData, mapped);

            var service = _fixture.GetTestableObject();
            var request = new GetVehicleAiDataRequest { RegistrationNumber = registration, SearchType = searchType, MetaData = metaData };

            // Act
            var result = await service.GetVehicleAiDataAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mapped.Type, result.Type);
            Assert.Equal(mapped.Content, result.Content);
            Assert.Equal(mapped.MetaData, result.MetaData);
            _fixture.VerifyMocks();
        }

        [Fact]
        public async Task GetVehicleAiDataAsync_ReturnsNull_WhenDbDataIsNull_AndNoDetailsAvailable()
        {
            // Arrange
            var registration = "ABC123";
            var searchType = AiType.Overview;
            var metaData = "meta";

            _fixture.MockGetAiDataAsync(registration, searchType, metaData, null);
            _fixture.MockGetVehicleDetailsAsync(registration, null);

            var service = _fixture.GetTestableObject();
            var request = new GetVehicleAiDataRequest { RegistrationNumber = registration, SearchType = searchType, MetaData = metaData };

            // Act
            var result = await service.GetVehicleAiDataAsync(request);

            // Assert
            Assert.Null(result);
            _fixture.VerifyMocks();
        }

        [Fact]
        public async Task GetVehicleAiDataAsync_ReturnsNull_WhenRegistrationIsNullOrEmpty()
        {
            // Arrange
            var service = _fixture.GetTestableObject();
            var request = new GetVehicleAiDataRequest { RegistrationNumber = null!, SearchType = AiType.Overview, MetaData = "meta" };

            // Act
            var result = await service.GetVehicleAiDataAsync(request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetVehicleAiDataAsync_ReturnsNull_WhenMetaDataIsNullOrEmpty_ForMotTestSummary()
        {
            // Arrange
            var service = _fixture.GetTestableObject();
            var request = new GetVehicleAiDataRequest { RegistrationNumber = "ABC123", SearchType = AiType.MotTestSummary, MetaData = null };

            // Act
            var result = await service.GetVehicleAiDataAsync(request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetVehicleAiDataAsync_ThrowsArgumentOutOfRangeException_ForInvalidAiType()
        {
            // Arrange
            var service = _fixture.GetTestableObject();
            var request = new GetVehicleAiDataRequest { RegistrationNumber = "ABC123", SearchType = (AiType)999, MetaData = "meta" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await service.GetVehicleAiDataAsync(request));
        }

        [Fact]
        public async Task GetVehicleAiDataAsync_ReturnsNull_WhenGeminiReturnsNull()
        {
            // Arrange
            var registration = "ABC123";
            var searchType = AiType.Overview;
            var metaData = "meta";
            var dbAiData = new AiDataDbModel
            {
                RegistrationNumber = registration,
                Type = searchType,
                MetaData = metaData,
                GeneratedText = "old-response",
                Updated = DateTime.UtcNow.AddDays(-2),
                DataHash = "old-hash"
            };

            _fixture.MockGetAiDataAsync(registration, searchType, metaData, dbAiData);
            _fixture.MockGetVehicleDetailsAsync(registration, new DetailsModel());
            _fixture.MockGetGeminiResponseAsync(It.IsAny<string>(), null!);

            var service = _fixture.GetTestableObject();
            var request = new GetVehicleAiDataRequest { RegistrationNumber = registration, SearchType = searchType, MetaData = metaData };

            // Act
            var result = await service.GetVehicleAiDataAsync(request);

            // Assert
            Assert.Null(result);
            _fixture.VerifyMocks();
        }

        [Fact]
        public async Task GetVehicleAiDataAsync_UpsertsAiData_WhenDataHashIsDifferent()
        {
            // Arrange
            var registration = "ABC123";
            var searchType = AiType.Overview;
            var metaData = "meta";
            var dbAiData = new AiDataDbModel
            {
                RegistrationNumber = registration,
                Type = searchType,
                MetaData = metaData,
                GeneratedText = "old-response",
                Updated = DateTime.UtcNow.AddDays(-2),
                DataHash = "old-hash"
            };
            var details = new DetailsModel();
            var aiResponse = "new-response";
            var newHash = "new-hash";
            var newDbAiData = new AiDataDbModel
            {
                RegistrationNumber = registration,
                Type = searchType,
                MetaData = metaData,
                GeneratedText = aiResponse,
                Updated = DateTime.UtcNow,
                DataHash = newHash
            };
            var mapped = new AiDataModel { Type = searchType, Content = aiResponse, MetaData = metaData };

            _fixture.MockGetAiDataAsync(registration, searchType, metaData, dbAiData);
            _fixture.MockGetVehicleDetailsAsync(registration, details);
            _fixture.MockGetGeminiResponseAsync(It.IsAny<string>(), aiResponse);
            _fixture.MockApiMapper_MapToAiDataDbModel(registration, searchType, metaData, aiResponse, It.IsAny<string>(), newDbAiData);
            _fixture.MockUpsertAiDataAsync(newDbAiData);
            _fixture.MockDatabaseMapper_MapAiData(newDbAiData, mapped);

            var service = _fixture.GetTestableObject();
            var request = new GetVehicleAiDataRequest { RegistrationNumber = registration, SearchType = searchType, MetaData = metaData };

            // Act
            var result = await service.GetVehicleAiDataAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mapped.Type, result.Type);
            Assert.Equal(mapped.Content, result.Content);
            Assert.Equal(mapped.MetaData, result.MetaData);
            _fixture.VerifyMocks();
        }
    }
}
