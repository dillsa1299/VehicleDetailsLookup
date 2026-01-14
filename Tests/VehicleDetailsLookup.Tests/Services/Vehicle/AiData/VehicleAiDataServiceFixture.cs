using Moq;
using VehicleDetailsLookup.Models.Database.AiData;
using VehicleDetailsLookup.Repositories.AiData;
using VehicleDetailsLookup.Services.Api.Gemini;
using VehicleDetailsLookup.Services.Mappers.ApiDatabase;
using VehicleDetailsLookup.Services.Mappers.DatabaseFrontend;
using VehicleDetailsLookup.Services.Vehicle.AiData;
using VehicleDetailsLookup.Services.Vehicle.Details;
using VehicleDetailsLookup.Services.Vehicle.Mot;
using VehicleDetailsLookup.Shared.Models.Ai;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Tests.Services.Vehicle.AiData
{
    public class VehicleAiDataServiceFixture
    {
        private Mock<IGeminiService> _geminiServiceMock = new();
        private Mock<IAiDataRepository> _aiDataRepositoryMock = new();
        private Mock<IVehicleDetailsService> _vehicleDetailsServiceMock = new();
        private Mock<IVehicleMotService> _vehicleMotServiceMock = new();
        private Mock<IApiDatabaseMapperService> _apiDatabaseMapperServiceMock = new();
        private Mock<IDatabaseFrontendMapperService> _databaseFrontendMapperServiceMock = new();

        public VehicleAiDataService GetTestableObject()
        {
            _geminiServiceMock = new Mock<IGeminiService>();
            _aiDataRepositoryMock = new Mock<IAiDataRepository>();
            _vehicleDetailsServiceMock = new Mock<IVehicleDetailsService>();
            _vehicleMotServiceMock = new Mock<IVehicleMotService>();
            _apiDatabaseMapperServiceMock = new Mock<IApiDatabaseMapperService>();
            _databaseFrontendMapperServiceMock = new Mock<IDatabaseFrontendMapperService>();

            return new VehicleAiDataService(
                _geminiServiceMock.Object,
                _aiDataRepositoryMock.Object,
                _vehicleDetailsServiceMock.Object,
                _vehicleMotServiceMock.Object,
                _apiDatabaseMapperServiceMock.Object,
                _databaseFrontendMapperServiceMock.Object
            );
        }

        public void VerifyMocks()
        {
            _geminiServiceMock.Verify();
            _geminiServiceMock.VerifyNoOtherCalls();
            _aiDataRepositoryMock.Verify();
            _aiDataRepositoryMock.VerifyNoOtherCalls();
            _vehicleDetailsServiceMock.Verify();
            _vehicleDetailsServiceMock.VerifyNoOtherCalls();
            _vehicleMotServiceMock.Verify();
            _vehicleMotServiceMock.VerifyNoOtherCalls();
            _apiDatabaseMapperServiceMock.Verify();
            _apiDatabaseMapperServiceMock.VerifyNoOtherCalls();
            _databaseFrontendMapperServiceMock.Verify();
            _databaseFrontendMapperServiceMock.VerifyNoOtherCalls();
        }

        public void MockGetGeminiResponseAsync(string prompt, string response)
        {
            _geminiServiceMock
                .Setup(x => x.GetGeminiResponseAsync(prompt))
                .ReturnsAsync(response)
                .Verifiable(Times.Once);
        }

        public void MockGetAiDataAsync(string registrationNumber, AiType searchType, string metaData, AiDataDbModel? response)
        {
            _aiDataRepositoryMock
                .Setup(x => x.GetAiDataAsync(registrationNumber, searchType, metaData))
                .ReturnsAsync(response)
                .Verifiable(Times.Once);
        }

        public void MockUpsertAiDataAsync(AiDataDbModel aiData)
        {
            _aiDataRepositoryMock
                .Setup(x => x.UpsertAiDataAsync(aiData))
                .Verifiable(Times.Once);
        }

        public void MockGetVehicleDetailsAsync(string registrationNumber, DetailsModel? response)
        {
            _vehicleDetailsServiceMock
                .Setup(x => x.GetVehicleDetailsAsync(registrationNumber, false))
                .ReturnsAsync(response)
                .Verifiable(Times.Once);
        }

        public void MockGetVehicleMotTestsAsync(string registrationNumber, IEnumerable<MotTestModel>? response)
        {
            _vehicleMotServiceMock
                .Setup(x => x.GetVehicleMotTestsAsync(registrationNumber))
                .ReturnsAsync(response)
                .Verifiable(Times.Once);
        }

        public void MockApiMapper_MapToAiDataDbModel(string registrationNumber, AiType type, string? metaData, string aiResponse, string dataHash, AiDataDbModel response)
        {
            _apiDatabaseMapperServiceMock
                .Setup(x => x.MapAiData(registrationNumber, type, metaData, aiResponse, dataHash))
                .Returns(response)
                .Verifiable(Times.Once);
        }

        public void MockDatabaseMapper_MapAiData(AiDataDbModel aiData, AiDataModel response)
        {
            _databaseFrontendMapperServiceMock
                .Setup(x => x.MapAiData(aiData))
                .Returns(response)
                .Verifiable(Times.Once);
        }
    }
}