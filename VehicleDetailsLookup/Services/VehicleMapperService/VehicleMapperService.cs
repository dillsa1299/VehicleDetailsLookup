using System.Globalization;
using VehicleDetailsLookup.Models.Enums;
using VehicleDetailsLookup.Models.SearchResponses;
using VehicleDetailsLookup.Models.SearchResponses.ImageSearch;
using VehicleDetailsLookup.Models.SearchResponses.MotSearch;
using VehicleDetailsLookup.Shared.Models;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Services.VehicleMapper
{
    public class VehicleMapperService: IVehicleMapperService
    {
        public VehicleModel MapDetails(VehicleModel vehicle, VesSearchResponse vesSearchResponse, MotSearchResponse motSearchResponse)
        {
            // VES response
            vehicle.RegistrationNumber = vesSearchResponse.RegistrationNumber ?? string.Empty;
            vehicle.YearOfManufacture = vesSearchResponse.YearOfManufacture;
            vehicle.Make = FormatName(vesSearchResponse.Make, 3);
            vehicle.Colour = FormatName(vesSearchResponse.Colour, 0);
            vehicle.EngineCapacity = $"{vesSearchResponse.EngineCapacity} cc";
            vehicle.FuelType = FormatName(vesSearchResponse.FuelType, 0);
            vehicle.TaxStatus = vesSearchResponse.TaxStatus ?? string.Empty;
            vehicle.MotStatus = vesSearchResponse.MotStatus ?? string.Empty;
            vehicle.MotExpiryDate = DateOnlyTryParse(vesSearchResponse.MotExpiryDate, "yyyy-MM-dd");
            vehicle.DateOfLastV5CIssued = DateOnlyTryParse(vesSearchResponse.DateOfLastV5CIssued, "yyyy-MM-dd") ?? default;
            vehicle.MonthOfFirstRegistration = DateOnlyTryParse(vesSearchResponse.MonthOfFirstRegistration, "yyyy-MM") ?? default;
            vehicle.TaxDueDate = DateOnlyTryParse(vesSearchResponse.TaxDueDate, "yyyy-MM-dd");

            // MOT response
            vehicle.Model = FormatName(motSearchResponse.Model, 3);
            if (motSearchResponse.MotTests.Any())
            {
                vehicle.MotTests = [.. motSearchResponse.MotTests.Select(test => new MotModel
                    {
                        CompletedDate = DateOnlyTryParseIso(test.CompletedDate) ?? default,
                        Passed = test.TestResult == "PASSED",
                        ExpiryDate = DateOnlyTryParse(test.ExpiryDate, "yyyy-MM-dd") ?? default,
                        OdometerValue = long.TryParse(test.OdometerValue, out var odo) ? odo : -1,
                        OdometerUnit = test.OdometerUnit == "KM" ? "Kilometers" : "Miles",
                        Defects = test.Defects?.Select(defect => new MotDefectModel
                        {
                            Type = GetDefectType(defect.Type),
                            Description = defect.Text,
                            Dangerous = defect.Dangerous
                        }).ToList() ?? []
                    })];
            }

            vehicle.DetailsLastUpdated = DateTime.UtcNow;

            return vehicle;
        }

        // Helper for title-casing with length check
        private static string FormatName(string? value, int maxLength) =>
            !string.IsNullOrWhiteSpace(value)
                ? (value.Length > maxLength ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLowerInvariant()) : value)
                : string.Empty;

        // Helper for safe date parsing
        private static DateOnly? DateOnlyTryParse(string? value, string format)
            => !string.IsNullOrWhiteSpace(value) && DateOnly.TryParseExact(value, format, out var date)
                ? date
                : null;

        // Helper for ISO date parsing
        private static DateOnly? DateOnlyTryParseIso(string? value)
            => DateTimeOffset.TryParse(value, out var dt) ? DateOnly.FromDateTime(dt.DateTime) : null;

        // Helper to map MOT defect types from string to enum
        private static MotDefectType GetDefectType(string? type)
        {
            return type switch
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

        public VehicleModel MapImages(VehicleModel vehicle, ImageSearchResponse imageSearchResponse)
        {
            if (imageSearchResponse == null || imageSearchResponse.Items == null || !imageSearchResponse.Items.Any())
            {
                vehicle.Images = Enumerable.Empty<ImageModel>();
                return vehicle;
            }

            IEnumerable<ImageModel> imagesModel = [];
            int i = 0;

            foreach (var item in imageSearchResponse.Items)
            {
                if (item != null && !string.IsNullOrWhiteSpace(item.Link))
                {
                    imagesModel = imagesModel.Append(new ImageModel
                    {
                        Index = i++,
                        Title = item.Title,
                        Url = item.Link
                    });
                }
            }

            vehicle.Images = imagesModel;

            vehicle.ImagesLastUpdated = DateTime.UtcNow;

            return vehicle;
        }
        public VehicleModel MapAI(VehicleModel vehicle, string aiResponse, AiSearchType searchType)
        {
            switch (searchType)
            {
                case AiSearchType.Overview:
                    vehicle.AiOverview = aiResponse ?? string.Empty;
                    break;
                case AiSearchType.CommonIssues:
                    vehicle.AiCommonIssues = aiResponse ?? string.Empty;
                    break;
                case AiSearchType.MotHistorySummary:
                    vehicle.AiMotHistorySummary = aiResponse ?? string.Empty;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }
            return vehicle;
        }
    }
}
