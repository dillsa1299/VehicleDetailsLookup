using VehicleDetailsLookup.Models.Database.Mot;
using VehicleDetailsLookup.Repositories;
using VehicleDetailsLookup.Services.Api.Mot;
using VehicleDetailsLookup.Services.Mappers;
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
            var dbMotTests = _motRepository.GetMotTests(registrationNumber);
            if (dbMotTests != null && dbMotTests.All(test => test.Updated > DateTime.UtcNow.AddMinutes(-15)))
            {
                // Return stored MOT tests if they are recent enough
                return _databaseMapper.MapMotTests(dbMotTests);
            }

            var motResponse = await _motService.GetMotResponseAsync(registrationNumber);
            if (motResponse == null || motResponse.MotTests == null)
                return null;

            dbMotTests = _apiMapper.MapMotTests(registrationNumber, motResponse.MotTests);
            _motRepository.UpdateMotTests(dbMotTests.Select(test => (MotTestDbModel)test));

            return _databaseMapper.MapMotTests(dbMotTests);
        }
    }
}
