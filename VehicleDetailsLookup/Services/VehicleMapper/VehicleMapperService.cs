using System.Globalization;
using VehicleDetailsLookup.Models.SearchResponses;
using VehicleDetailsLookup.Models.SearchResponses.ImageSearch;
using VehicleDetailsLookup.Models.SearchResponses.MotSearch;
using VehicleDetailsLookup.Shared.Models;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Services.VehicleMapper
{
    /// <summary>
    /// Provides mapping operations to convert various vehicle search responses into a <see cref="VehicleModel"/>.
    /// </summary>
    public class VehicleMapperService : IVehicleMapperService
    {
        /// <summary>
        /// Maps VES and MOT search responses to a <see cref="VehicleModel"/> containing comprehensive vehicle details.
        /// </summary>
        /// <param name="vehicle">The <see cref="VehicleModel"/> instance to populate with details.</param>
        /// <param name="vesSearchResponse">The <see cref="VesSearchResponse"/> containing vehicle and tax details.</param>
        /// <param name="motSearchResponse">The <see cref="MotSearchResponse"/> containing MOT history and details.</param>
        /// <returns>
        /// The updated <see cref="VehicleModel"/> populated with details from the provided VES and MOT responses.
        /// </returns>
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

        /// <summary>
        /// Formats a string value to title case if its length exceeds a specified maximum length.
        /// </summary>
        /// <param name="value">The string value to format.</param>
        /// <param name="maxLength">The maximum length before formatting is applied.</param>
        /// <returns>The formatted string, or an empty string if the input is null or whitespace.</returns>
        private static string FormatName(string? value, int maxLength) =>
            !string.IsNullOrWhiteSpace(value)
                ? value.Length > maxLength ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLowerInvariant()) : value
                : string.Empty;

        /// <summary>
        /// Attempts to parse a string into a <see cref="DateOnly"/> using the specified format.
        /// </summary>
        /// <param name="value">The date string to parse.</param>
        /// <param name="format">The expected date format.</param>
        /// <returns>A nullable <see cref="DateOnly"/> if parsing succeeds; otherwise, null.</returns>
        private static DateOnly? DateOnlyTryParse(string? value, string format)
            => !string.IsNullOrWhiteSpace(value) && DateOnly.TryParseExact(value, format, out var date)
                ? date
                : null;

        /// <summary>
        /// Attempts to parse a string in ISO date format into a <see cref="DateOnly"/>.
        /// </summary>
        /// <param name="value">The ISO date string to parse.</param>
        /// <returns>A nullable <see cref="DateOnly"/> if parsing succeeds; otherwise, null.</returns>
        private static DateOnly? DateOnlyTryParseIso(string? value)
            => DateTimeOffset.TryParse(value, out var dt) ? DateOnly.FromDateTime(dt.DateTime) : null;

        /// <summary>
        /// Maps a MOT defect type string to the corresponding <see cref="MotDefectType"/> enum value.
        /// </summary>
        /// <param name="type">The defect type string.</param>
        /// <returns>The corresponding <see cref="MotDefectType"/> value.</returns>
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

        /// <summary>
        /// Maps an image search response to a <see cref="VehicleModel"/> containing image information.
        /// </summary>
        /// <param name="vehicle">The <see cref="VehicleModel"/> instance to populate with image data.</param>
        /// <param name="imageSearchResponse">The <see cref="ImageSearchResponse"/> containing image items.</param>
        /// <returns>
        /// The updated <see cref="VehicleModel"/> with image data populated from the image search response.
        /// </returns>
        public VehicleModel MapImages(VehicleModel vehicle, ImageSearchResponse imageSearchResponse)
        {
            if (imageSearchResponse == null || imageSearchResponse.Items == null || !imageSearchResponse.Items.Any())
            {
                vehicle.Images = [];
                return vehicle;
            }

            IEnumerable<ImageModel> imagesModel = [];
            int i = 1;

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

        /// <summary>
        /// Maps an AI-generated response to a property of <see cref="VehicleModel"/> based on the specified <see cref="VehicleAiType"/>.
        /// </summary>
        /// <param name="vehicle">The <see cref="VehicleModel"/> instance to update with AI-generated data.</param>
        /// <param name="aiResponse">The <see cref="AiSearchResponse"/> containing the AI-generated content, such as summary, issues, or MOT history.</param>
        /// <param name="searchType">The <see cref="VehicleAiType"/> indicating which property or section of <see cref="VehicleModel"/> to populate (e.g., overview, common issues, MOT summary).</param>
        /// <returns>
        /// The updated <see cref="VehicleModel"/> with the relevant AI-generated information set according to the specified <paramref name="searchType"/>.
        /// </returns>
        public VehicleModel MapAI(VehicleModel vehicle, AiSearchResponse aiResponse, VehicleAiType searchType)
        {
            var aiDataList = vehicle.AiData.Where(ai => ai.Type != searchType).ToList();

            aiDataList.Add(new VehicleAiDataModel
            {
                LastUpdated = DateTime.UtcNow,
                Type = searchType,
                Content = aiResponse.Response
            });

            vehicle.AiData = aiDataList;

            return vehicle;
        }
    }
}
