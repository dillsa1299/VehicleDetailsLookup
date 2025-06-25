using VehicleDetailsLookup.Repositories.Mot;
using VehicleDetailsLookup.Services.Api.Mot;
using VehicleDetailsLookup.Services.Mappers.ApiDatabase;
using VehicleDetailsLookup.Services.Mappers.DatabaseFrontend;
using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Services.Vehicle.Mot
{
    public class VehicleMotService(IMotRepository motRepository, IMotService motService, IApiDatabaseMapperService apiMapper, IDatabaseFrontendMapperService databaseMapper) : IVehicleMotService
    {
        private readonly IMotRepository _motRepository = motRepository
            ?? throw new ArgumentNullException(nameof(motRepository));
        private readonly IMotService _motService = motService
            ?? throw new ArgumentNullException(nameof(motService));
        private readonly IApiDatabaseMapperService _apiMapper = apiMapper
            ?? throw new ArgumentNullException(nameof(apiMapper));
        private readonly IDatabaseFrontendMapperService _databaseMapper = databaseMapper
            ?? throw new ArgumentNullException(nameof(databaseMapper));

        public async ValueTask<IEnumerable<IMotTestModel>?> GetVehicleMotTestsAsync(string registrationNumber)
        {
            // Check if the MOT tests are already stored in the database
            var dbMotTests = await _motRepository.GetMotTestsAsync(registrationNumber);
            if (dbMotTests != null && dbMotTests.Any() && dbMotTests.All(test => test.Updated > DateTime.UtcNow.AddMinutes(-15)))
            {
                // Return stored MOT tests if they are recent enough
                return _databaseMapper.MapMotTests(dbMotTests);
            }

            // If not found or too old, fetch from the MOT API
            var motResponse = await _motService.GetMotResponseAsync(registrationNumber);

            if (motResponse == null || motResponse.MotTests == null || !motResponse.MotTests.Any())
                // If no MOT tests are found, return null
                return null;

            // Map the API response to database models and update the repository
            dbMotTests = _apiMapper.MapMotTests(registrationNumber, motResponse.MotTests);
            await _motRepository.UpdateMotTestsAsync(dbMotTests);

            return _databaseMapper.MapMotTests(dbMotTests);
        }
    }
}
