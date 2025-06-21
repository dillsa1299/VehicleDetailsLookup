using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using VehicleDetailsLookup.Models.ApiResponses.Mot;
using VehicleDetailsLookup.Models.ApiResponses.Ves;
using VehicleDetailsLookup.Models.Database.Details;
using VehicleDetailsLookup.Repositories;
using VehicleDetailsLookup.Services.Api.Mot;
using VehicleDetailsLookup.Services.Api.Ves;
using VehicleDetailsLookup.Services.Mappers;
using VehicleDetailsLookup.Services.Vehicle.Details;
using VehicleDetailsLookup.Shared.Models.Details;

namespace VehicleDetailsLookup.Services.Vehicle.VehicleDetails
{
    public class VehicleDetailsService(IDetailsRepository detailsRepository, IVesService vesService, IMotService motService, IApiDatabaseMapperService apiMapper, IDatabaseFrontendMapperService databaseMapper) : IVehicleDetailsService
    {
        private readonly IDetailsRepository _detailsRepository = detailsRepository
            ?? throw new ArgumentNullException(nameof(detailsRepository));
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
            var dbDetails = _detailsRepository.GetDetails(registrationNumber);

            if (dbDetails?.Updated > DateTime.UtcNow.AddMinutes(-15))
            {
                // Return stored vehicle details if they are recent enough
                return _databaseMapper.MapDetails(dbDetails);
            }

            var vesResponse = await _vesService.GetVesResponseAsync(registrationNumber);
            if (vesResponse == null)
                return null;

            var motResponse = await _motService.GetMotResponseAsync(registrationNumber);
            if (motResponse == null)
                return null;

            dbDetails = _apiMapper.MapDetails(vesResponse, motResponse);
            _detailsRepository.UpdateDetails((DetailsDbModel)dbDetails);

            return _databaseMapper.MapDetails(dbDetails);
        }
    }
}
