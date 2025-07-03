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
            // Get distinct until changed recent lookups. Prevents multiple entries for the same registration number in a row.
            var sql = $@"
                SELECT l.*
                FROM (
                    SELECT *,
                           LAG(RegistrationNumber) OVER (ORDER BY [DateTime] DESC) AS PrevReg
                    FROM Lookups
                ) l
                WHERE l.RegistrationNumber <> COALESCE(l.PrevReg, '')
                ORDER BY l.[DateTime] DESC
                LIMIT {count}
            ";

            var lookups = await _dbContext.Lookups
                .FromSqlRaw(sql)
                .Include(l => l.Details)
                .ToListAsync();

            return lookups;
        }

        public async ValueTask<IEnumerable<ILookupDbModel>?> GetRecentLookupsAsync(string registrationNumber, int count = 0)
        {
            var query = _dbContext.Lookups
                .Include(l => l.Details)
                .Where(l => l.RegistrationNumber == registrationNumber)
                .OrderByDescending(l => l.DateTime);

            if (count > 0)
                return await query.Take(count).ToListAsync();

            return await query.ToListAsync();
        }

        public async ValueTask<int> GetVehicleLookupCountAsync(string registrationNumber)
        {
            return await _dbContext.Lookups.CountAsync(l => l.RegistrationNumber == registrationNumber);
        }
    }
}