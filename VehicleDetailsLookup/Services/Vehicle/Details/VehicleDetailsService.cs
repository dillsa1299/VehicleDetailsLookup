using VehicleDetailsLookup.Repositories.Details;
using VehicleDetailsLookup.Repositories.Lookup;
using VehicleDetailsLookup.Services.Api.Mot;
using VehicleDetailsLookup.Services.Api.Ves;
using VehicleDetailsLookup.Services.Mappers.ApiDatabase;
using VehicleDetailsLookup.Services.Mappers.DatabaseFrontend;
using VehicleDetailsLookup.Shared.Models.Details;

namespace VehicleDetailsLookup.Services.Vehicle.Details
{
    public class VehicleDetailsService(IDetailsRepository detailsRepository, ILookupRepository lookupRepository, IVesService vesService, IMotService motService, IApiDatabaseMapperService apiMapper, IDatabaseFrontendMapperService databaseMapper) : IVehicleDetailsService
    {
        private readonly IDetailsRepository _detailsRepository = detailsRepository
            ?? throw new ArgumentNullException(nameof(detailsRepository));
        private readonly ILookupRepository _lookupRepository = lookupRepository
            ?? throw new ArgumentNullException(nameof(lookupRepository));
        private readonly IVesService _vesService = vesService
            ?? throw new ArgumentNullException(nameof(vesService));
        private readonly IMotService _motService = motService
            ?? throw new ArgumentNullException(nameof(motService));
        private readonly IApiDatabaseMapperService _apiMapper = apiMapper
            ?? throw new ArgumentNullException(nameof(apiMapper));
        private readonly IDatabaseFrontendMapperService _databaseMapper = databaseMapper
            ?? throw new ArgumentNullException(nameof(databaseMapper));
        
        public async ValueTask<IDetailsModel?> GetVehicleDetailsAsync(string registrationNumber)
        {
            // Check if the vehicle details are already stored in the database
            var dbDetails = await _detailsRepository.GetDetailsAsync(registrationNumber);

            if (dbDetails?.Updated > DateTime.UtcNow.AddMinutes(-15))
            {
                // Return stored vehicle details if they are recent enough
                return _databaseMapper.MapDetails(dbDetails);
            }

            // If not found or too old, fetch from the VES and MOT APIs
            var vesResponse = await _vesService.GetVesResponseAsync(registrationNumber);
            if (vesResponse == null)
                return null;

            var motResponse = await _motService.GetMotResponseAsync(registrationNumber);
            if (motResponse == null)
                return null;

            // Map the API responses to database models and update the repository
            dbDetails = _apiMapper.MapDetails(vesResponse, motResponse);
            await _detailsRepository.UpdateDetailsAsync(dbDetails);

            // Log the lookup
            await lookupRepository.AddLookupAsync(registrationNumber);

            return _databaseMapper.MapDetails(dbDetails);
        }
    }
}
