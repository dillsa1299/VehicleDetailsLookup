using Microsoft.EntityFrameworkCore;
using VehicleDetailsLookup.Models.Database;
using VehicleDetailsLookup.Models.Database.AiData;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Repositories.AiData
{
    public class AiDataRepository(VehicleDbContext dbContext) : IAiDataRepository
    {
        private readonly VehicleDbContext _dbContext = dbContext;

        public async Task UpdateAiDataAsync(IAiDataDbModel aiData)
        {
            // Try to find existing AI data for the given registration number and type
            var existing = await _dbContext.AiData
                .FirstOrDefaultAsync(x => x.RegistrationNumber == aiData.RegistrationNumber && x.Type == aiData.Type);

            if (existing != null)
            {
                // Update existing record
                existing.GeneratedText = aiData.GeneratedText;
                existing.Updated = DateTime.UtcNow;
                _dbContext.AiData.Update(existing);
            }
            else
            {
                // Add new record
                await _dbContext.AiData.AddAsync((AiDataDbModel)aiData);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<IAiDataDbModel?> GetAiDataAsync(string registrationNumber, AiType type)
        {
            return await _dbContext.AiData
                .FirstOrDefaultAsync(x => x.RegistrationNumber == registrationNumber && x.Type == type);
        }
    }
}