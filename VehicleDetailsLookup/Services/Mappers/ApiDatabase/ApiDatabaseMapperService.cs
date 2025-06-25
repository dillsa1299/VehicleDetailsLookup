using VehicleDetailsLookup.Models.ApiResponses.Gemini;
using VehicleDetailsLookup.Models.ApiResponses.GoogleImage;
using VehicleDetailsLookup.Models.ApiResponses.Mot;
using VehicleDetailsLookup.Models.ApiResponses.Ves;
using VehicleDetailsLookup.Models.Database.AiData;
using VehicleDetailsLookup.Models.Database.Details;
using VehicleDetailsLookup.Models.Database.Image;
using VehicleDetailsLookup.Models.Database.Mot;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Services.Mappers.ApiDatabase
{
    public class ApiDatabaseMapperService : IApiDatabaseMapperService
    {
        public IDetailsDbModel MapDetails(IVesResponse vesResponse, IMotResponse motResponse)
        {
            return new DetailsDbModel
            {
                RegistrationNumber = vesResponse.RegistrationNumber,
                YearOfManufacture = vesResponse.YearOfManufacture,
                Make = vesResponse.Make,
                Model = motResponse?.Model ?? string.Empty,
                Colour = vesResponse.Colour,
                EngineCapacity = vesResponse.EngineCapacity > 0 ? $"{vesResponse.EngineCapacity} cc" : null,
                FuelType = vesResponse.FuelType,
                TaxStatus = vesResponse.TaxStatus,
                TaxDueDate = DateOnlyTryParse(vesResponse.TaxDueDate, "yyyy-MM-dd"),
                MotStatus = vesResponse.MotStatus,
                MotExpiryDate = DateOnlyTryParse(vesResponse.MotExpiryDate, "yyyy-MM-dd"),
                DateOfLastV5CIssued = DateOnlyTryParse(vesResponse.DateOfLastV5CIssued, "yyyy-MM-dd") ?? default,
                MonthOfFirstRegistration = DateOnlyTryParse(vesResponse.MonthOfFirstRegistration, "yyyy-MM") ?? default,
                Updated = DateTime.UtcNow,
                Lookups = []
            };
        }

        public IEnumerable<IMotTestDbModel> MapMotTests(string registrationNumber, IEnumerable<IMotResponseTest> motTests)
        {
            if (motTests == null)
                return [];
            return motTests.Select(test => new MotTestDbModel
            {
                RegistrationNumber = registrationNumber,
                TestNumber = test.MotTestNumber,
                CompletedDate = DateOnlyTryParseIso(test.CompletedDate) ?? default,
                Passed = string.Equals(test.TestResult, "PASSED", StringComparison.OrdinalIgnoreCase),
                ExpiryDate = DateOnlyTryParse(test.ExpiryDate, "yyyy-MM-dd") ?? default,
                OdometerValue = long.TryParse(test.OdometerValue, out var odo) ? odo : -1,
                OdometerUnit = test.OdometerUnit,
                MotDefects = [.. MapMotDefects(test.MotTestNumber, test.Defects)],
                Updated = DateTime.UtcNow
            });
        }

        private static IEnumerable<MotDefectDbModel> MapMotDefects(string testNumber, IEnumerable<IMotResponseDefect> motDefects)
        {
            if (motDefects == null)
                return [];

            // Deduplicate defects
            motDefects = motDefects
                .GroupBy(d => new { d.Text, d.Type, d.Dangerous })
                .Select(g => g.First());

            return motDefects.Select(defect => new MotDefectDbModel
            {
                TestNumber = testNumber,
                Description = defect.Text,
                Type = GetDefectType(defect.Type),
                Dangerous = defect.Dangerous
            });
        }

        public IEnumerable<IImageDbModel> MapImages(string registrationNumber, IEnumerable<IGoogleImageResponseItem> images)
        {
            if (images == null)
                return [];

            var now = DateTime.UtcNow;
            return images
                .Where(img => !string.IsNullOrWhiteSpace(img.Link))
                .Select(img => new ImageDbModel
                {
                    RegistrationNumber = registrationNumber,
                    Title = img.Title,
                    Url = img.Link,
                    Updated = now
                });
        }

        public IAiDataDbModel MapAiData(string registrationNumber, AiType type, IGeminiResponse geminiResponse)
        {
            return new AiDataDbModel
            {
                RegistrationNumber = registrationNumber,
                Type = type,
                GeneratedText = geminiResponse?.Response,
                Updated = DateTime.UtcNow
            };
        }

        // --- Helpers ---
        private static DateOnly? DateOnlyTryParse(string? value, string format)
            => !string.IsNullOrWhiteSpace(value) && DateOnly.TryParseExact(value, format, out var date)
                ? date
                : null;

        private static DateOnly? DateOnlyTryParseIso(string? value)
            => DateTimeOffset.TryParse(value, out var dt) ? DateOnly.FromDateTime(dt.DateTime) : null;

        private static MotDefectType GetDefectType(string? type)
        {
            return type?.ToUpperInvariant() switch
            {
                "ADVISORY" => MotDefectType.Advisory,
                "DANGEROUS" => MotDefectType.Dangerous,
                "FAIL" => MotDefectType.Fail,
                "MAJOR" => MotDefectType.Major,
                "MINOR" => MotDefectType.Minor,
                "NON SPECIFIC" => MotDefectType.NonSpecific,
                "SYSTEM GENERATED" => MotDefectType.SystemGenerated,
                "USER ENTERED" => MotDefectType.UserEntered,
                _ => MotDefectType.UserEntered
            };
        }
    }
}
