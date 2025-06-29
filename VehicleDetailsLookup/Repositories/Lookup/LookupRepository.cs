using VehicleDetailsLookup.Models.Database.Lookup;
using VehicleDetailsLookup.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace VehicleDetailsLookup.Repositories.Lookup
{
    public class LookupRepository(VehicleDbContext dbContext) : ILookupRepository
    {
        private readonly VehicleDbContext _dbContext = dbContext;

        public async Task AddLookupAsync(string registrationNumber)
        {
            var lookup = new LookupDbModel
            {
                RegistrationNumber = registrationNumber,
                DateTime = DateTime.UtcNow
            };
            await _dbContext.Lookups.AddAsync(lookup);
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<IEnumerable<ILookupDbModel>?> GetRecentLookupsAsync(int count)
        {
            return await _dbContext.Lookups
                .Include(l => l.Details)
                .OrderByDescending(l => l.DateTime)
                .Take(count)
                .ToListAsync();
        }

        public async ValueTask<IEnumerable<ILookupDbModel>?> GetRecentLookupsAsync(string registrationNumber, int count)
        {
            return await _dbContext.Lookups
                .Include(l => l.Details)
                .Where(l => l.RegistrationNumber == registrationNumber)
                .OrderByDescending(l => l.DateTime)
                .Take(count)
                .ToListAsync();
        }

        public async ValueTask<int> GetVehicleLookupCountAsync(string registrationNumber)
        {
            return await _dbContext.Lookups.CountAsync(l => l.RegistrationNumber == registrationNumber);
        }
    }
}