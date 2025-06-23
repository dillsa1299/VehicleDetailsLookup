using VehicleDetailsLookup.Models.Database.AiData;
using VehicleDetailsLookup.Models.Database.Details;
using VehicleDetailsLookup.Models.Database.Image;
using VehicleDetailsLookup.Models.Database.Lookup;
using VehicleDetailsLookup.Models.Database.Mot;
using VehicleDetailsLookup.Shared.Models.Ai;
using VehicleDetailsLookup.Shared.Models.Details;
using VehicleDetailsLookup.Shared.Models.Image;
using VehicleDetailsLookup.Shared.Models.Lookup;
using VehicleDetailsLookup.Shared.Models.Mot;

namespace VehicleDetailsLookup.Services.Mappers.DatabaseFrontend
{
    public class DatabaseFrontendMapperService : IDatabaseFrontendMapperService
    {
        public IDetailsModel MapDetails(IDetailsDbModel details)
        {
            // TODO: Probably need to handle capitalised fields here. E.g. BMW, Audi TT etc.
            return new DetailsModel
            {
                RegistrationNumber = details.RegistrationNumber ?? string.Empty,
                YearOfManufacture = details.YearOfManufacture,
                Make = details.Make ?? string.Empty,
                Model = details.Model ?? string.Empty,
                Colour = details.Colour ?? string.Empty,
                EngineCapacity = details.EngineCapacity ?? string.Empty,
                FuelType = details.FuelType ?? string.Empty,
                TaxStatus = details.TaxStatus ?? string.Empty,
                TaxDueDate = details.TaxDueDate,
                MotStatus = details.MotStatus ?? string.Empty,
                MotExpiryDate = details.MotExpiryDate,
                DateOfLastV5CIssued = details.DateOfLastV5CIssued,
                MonthOfFirstRegistration = details.MonthOfFirstRegistration
            };
        }

        public IEnumerable<IMotTestModel> MapMotTests(IEnumerable<IMotTestDbModel> motTests)
        {
            if (motTests == null)
                return [];

            return motTests.Select(test => new MotTestModel
            {
                CompletedDate = test.CompletedDate,
                Passed = test.Passed,
                ExpiryDate = test.ExpiryDate,
                OdometerValue = test.OdometerValue,
                OdometerUnit = test.OdometerUnit ?? string.Empty,
                Defects = MapMotDefects(test.MotDefects)
            });
        }

        private static IEnumerable<IMotDefectModel> MapMotDefects(IEnumerable<IMotDefectDbModel> motDefects)
        {
            if (motDefects == null)
                return [];

            return motDefects.Select(defect => new MotDefectModel
            {
                Description = defect.Description ?? string.Empty,
                Type = defect.Type,
                Dangerous = defect.Dangerous
            });
        }

        public IEnumerable<IImageModel> MapImages(IEnumerable<IImageDbModel> images)
        {
            if (images == null)
                return [];

            int i = 1;
            return images.Select(img => new ImageModel
            {
                Index = i++,
                Title = img.Title,
                Url = img.Url
            });
        }

        public IAiDataModel MapAiData(IAiDataDbModel aiData)
        {
            return new AiDataModel
            {
                Type = aiData.Type,
                Content = aiData.GeneratedText ?? string.Empty
            };
        }

        public ILookupModel MapLookup(ILookupDbModel lookup)
        {
            return new LookupModel
            {
                DateTime = lookup.DateTime,
                RegistrationNumber = lookup.RegistrationNumber ?? string.Empty,
                VehicleDetails = lookup.Details == null ? new DetailsModel(): MapDetails(lookup.Details)
            };
        }
    }
}
