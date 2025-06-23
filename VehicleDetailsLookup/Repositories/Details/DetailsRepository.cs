using VehicleDetailsLookup.Models.Database.Details;
using VehicleDetailsLookup.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace VehicleDetailsLookup.Repositories.Details
{
    public class DetailsRepository(VehicleDbContext dbContext) : IDetailsRepository
    {
        private readonly VehicleDbContext _dbContext = dbContext;

        public async Task UpdateDetailsAsync(IDetailsDbModel details)
        {
            var existing = await _dbContext.Details
                .FirstOrDefaultAsync(d => d.RegistrationNumber == details.RegistrationNumber);

            if (existing != null)
            {
                // Update details that can change
                existing.DateOfLastV5CIssued = details.DateOfLastV5CIssued;
                existing.MotExpiryDate = details.MotExpiryDate;
                existing.MotStatus = details.MotStatus;
                existing.TaxDueDate = details.TaxDueDate;
                existing.TaxStatus = details.TaxStatus;
                existing.Updated = DateTime.UtcNow;
                _dbContext.Details.Update(existing);
            }
            else
            {
                // Add new record
                await _dbContext.Details.AddAsync((DetailsDbModel)details);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<IDetailsDbModel?> GetDetailsAsync(string registrationNumber)
        {
            return await _dbContext.Details
                .Include(d => d.Lookups)
                .FirstOrDefaultAsync(d => d.RegistrationNumber == registrationNumber);
        }
    }
}
