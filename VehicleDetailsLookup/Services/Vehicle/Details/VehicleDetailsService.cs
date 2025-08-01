﻿using VehicleDetailsLookup.Repositories.Details;
using VehicleDetailsLookup.Repositories.Lookup;
using VehicleDetailsLookup.Repositories.Mot;
using VehicleDetailsLookup.Services.Api.Mot;
using VehicleDetailsLookup.Services.Api.Ves;
using VehicleDetailsLookup.Services.Mappers.ApiDatabase;
using VehicleDetailsLookup.Services.Mappers.DatabaseFrontend;
using VehicleDetailsLookup.Shared.Models.Details;

namespace VehicleDetailsLookup.Services.Vehicle.Details
{
    public class VehicleDetailsService(IDetailsRepository detailsRepository, IMotRepository motRepository, ILookupRepository lookupRepository, IVesService vesService, IMotService motService, IApiDatabaseMapperService apiMapper, IDatabaseFrontendMapperService databaseMapper) : IVehicleDetailsService
    {
        private readonly IDetailsRepository _detailsRepository = detailsRepository
            ?? throw new ArgumentNullException(nameof(detailsRepository));
        private readonly IMotRepository _motRepository = motRepository
            ?? throw new ArgumentNullException(nameof(motRepository));
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

        public async ValueTask<DetailsModel?> GetVehicleDetailsAsync(string registrationNumber, bool primarySearch = false)
        {
            // Check if the vehicle details are already stored in the database
            var dbDetails = await _detailsRepository.GetDetailsAsync(registrationNumber);

            if (dbDetails?.Updated > DateTime.UtcNow.AddMinutes(-15))
            {
                // Log the lookup if it was a user-initiated search instead of a background details load for additional information
                if (primarySearch)
                    await _lookupRepository.AddLookupAsync(registrationNumber);

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

            // Store the MOT tests whilst we've already got the data
            var dbMotTests = _apiMapper.MapMotTests(registrationNumber, motResponse.MotTests);
            await _motRepository.UpdateMotTestsAsync(dbMotTests);

            // Log the lookup if it was a user-initiated search instead of a background details load for additional information
            if (primarySearch)
                await _lookupRepository.AddLookupAsync(registrationNumber);

            return _databaseMapper.MapDetails(dbDetails);
        }
    }
}
