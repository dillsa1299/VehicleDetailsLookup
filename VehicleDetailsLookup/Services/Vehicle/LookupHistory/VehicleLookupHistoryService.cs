using VehicleDetailsLookup.Repositories.Lookup;
using VehicleDetailsLookup.Services.Mappers.DatabaseFrontend;
using VehicleDetailsLookup.Shared.Models.Lookup;

namespace VehicleDetailsLookup.Services.Vehicle.LookupHistory
{
    public class VehicleLookupHistoryService(ILookupRepository lookupRepository, IDatabaseFrontendMapperService databaseMapper) : IVehicleLookupHistoryService
    {
        private readonly ILookupRepository _lookupRepository = lookupRepository
            ?? throw new ArgumentNullException(nameof(lookupRepository));
        private readonly IDatabaseFrontendMapperService _databaseMapper = databaseMapper
            ?? throw new ArgumentNullException(nameof(databaseMapper));

        public async ValueTask<int> GetVehicleLookupCountAsync(string registrationNumber)
        {
            int count = await _lookupRepository.GetVehicleLookupCountAsync(registrationNumber);
            return count;
        }

        public async ValueTask<IEnumerable<LookupModel>?> GetRecentLookupsAsync(int count)
        {
            var recentLookupsDb = await _lookupRepository.GetRecentLookupsAsync(count);
            var recentLookups = recentLookupsDb?
                .Select(lookup => _databaseMapper.MapLookup(lookup))
                .ToList();

            return recentLookups;
        }

        public async ValueTask<IEnumerable<LookupModel>?> GetRecentLookupsAsync(string registrationNumber, int count = 0)
        {
            var recentLookupsDb = await _lookupRepository.GetRecentLookupsAsync(registrationNumber);
            var recentLookups = recentLookupsDb?
                .Select(lookup => _databaseMapper.MapLookup(lookup))
                .ToList();

            return recentLookups;
        }


    }
}
