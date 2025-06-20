using VehicleDetailsLookup.Models.Database.Mot;
using VehicleDetailsLookup.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace VehicleDetailsLookup.Repositories
{
    public class MotRepository(VehicleDbContext dbContext) : IMotRepository
    {
        private readonly VehicleDbContext _dbContext = dbContext;

        public void UpdateMotHistory(IEnumerable<MotTestDbModel> motTests)
        {
            foreach (var motTest in motTests)
            {
                if (!_dbContext.MotTests.Any(m => m.TestNumber == motTest.TestNumber))
                {
                    // If the MOT test does not exist, add it
                    _dbContext.MotTests.Add(motTest);
                }
            }
            _dbContext.SaveChanges();
        }

        public IEnumerable<IMotTestDbModel>? GetMotHistory(string registrationNumber)
        {
            return _dbContext.MotTests
                .Include(m => m.MotDefects)
                .Where(m => m.RegistrationNumber == registrationNumber)
                .OrderByDescending(m => m.CompletedDate)
                .ToList();
        }
    }
}
