using VehicleDetailsLookup.Models.Database.Details;
using VehicleDetailsLookup.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace VehicleDetailsLookup.Repositories
{
    public class DetailsRepository(VehicleDbContext dbContext) : IDetailsRepository
    {
        private readonly VehicleDbContext _dbContext = dbContext;

        public void UpdateDetails(DetailsDbModel details)
        {
            var existing = _dbContext.Details
                .FirstOrDefault(d => d.RegistrationNumber == details.RegistrationNumber);

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
                _dbContext.Details.Add(details);
            }
            _dbContext.SaveChanges();
        }

        public IDetailsDbModel? GetDetails(string registrationNumber)
        {
            return _dbContext.Details
                .Include(d => d.Lookups)
                .FirstOrDefault(d => d.RegistrationNumber == registrationNumber);
        }
    }
}
