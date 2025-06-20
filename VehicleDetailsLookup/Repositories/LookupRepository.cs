using VehicleDetailsLookup.Models.Database.Lookup;
using VehicleDetailsLookup.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace VehicleDetailsLookup.Repositories
{
    public class LookupRepository(VehicleDbContext dbContext) : ILookupRepository
    {
        private readonly VehicleDbContext _dbContext = dbContext;

        public void AddLookup(string registrationNumber)
        {
            var lookup = new LookupDbModel
            {
                RegistrationNumber = registrationNumber,
                DateTime = DateTime.UtcNow
            };
            _dbContext.Lookups.Add(lookup);
            _dbContext.SaveChanges();
        }

        public IEnumerable<ILookupDbModel>? GetRecentLookups(int count)
        {
            return _dbContext.Lookups
                .Include(l => l.Details)
                .OrderByDescending(l => l.DateTime)
                .Take(count)
                .ToList();
        }

        public IEnumerable<ILookupDbModel>? GetVehicleLookups(string registrationNumber)
        {
            return _dbContext.Lookups
                .Include(l => l.Details)
                .Where(l => l.RegistrationNumber == registrationNumber)
                .OrderByDescending(l => l.DateTime)
                .ToList();
        }

        public int GetVehicleLookupCount(string registrationNumber)
        {
            return _dbContext.Lookups.Count(l => l.RegistrationNumber == registrationNumber);
        }
    }
}