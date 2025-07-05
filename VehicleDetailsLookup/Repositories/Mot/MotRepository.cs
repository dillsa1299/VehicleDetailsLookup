using VehicleDetailsLookup.Models.Database.Mot;
using VehicleDetailsLookup.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace VehicleDetailsLookup.Repositories.Mot
{
    public class MotRepository(VehicleDbContext dbContext) : IMotRepository
    {
        private readonly VehicleDbContext _dbContext = dbContext;

        public async Task UpdateMotTestsAsync(IEnumerable<MotTestDbModel> motTests)
        {
            if (motTests == null || !motTests.Any())
                return;

            foreach (var motTest in motTests)
            {
                if (!await _dbContext.MotTests.AnyAsync(m => m.TestNumber == motTest.TestNumber))
                {
                    // If the MOT test does not exist, add it
                    await _dbContext.MotTests.AddAsync(motTest);
                }
                else
                {
                    // If the mot test exists update the Updated field
                    var existingMotTest = await _dbContext.MotTests.FirstAsync(m => m.TestNumber == motTest.TestNumber);
                    existingMotTest.Updated = motTest.Updated;
                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<IEnumerable<MotTestDbModel>?> GetMotTestsAsync(string registrationNumber)
        {
            return await _dbContext.MotTests
                .Include(m => m.MotDefects)
                .Where(m => m.RegistrationNumber == registrationNumber)
                .OrderByDescending(m => m.CompletedDate)
                .ToListAsync();
        }
    }
}
