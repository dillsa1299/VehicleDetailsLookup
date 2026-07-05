using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.State;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails;

public partial class VehicleDetails
{
    private VehicleModel? Vehicle => LookupState.Vehicle;

    private bool IsSearchingDetails => LookupState.IsSearching(VehicleLookupType.Details);
    private bool IsSearchingMotHistory => LookupState.IsSearching(VehicleLookupType.MotHistory);
    private bool IsSearchingImages => LookupState.IsSearching(VehicleLookupType.Images);
    private bool IsSearchingAiOverview => LookupState.IsSearching(VehicleLookupType.AiOverview);
    private bool IsSearchingAiCommonIssues => LookupState.IsSearching(VehicleLookupType.AiCommonIssues);
    private bool IsSearchingAiMotHistorySummary => LookupState.IsSearching(VehicleLookupType.AiMotHistorySummary);

    private const string _placeholderImage = "images/placeholder-car.svg";
    private const string _aiFailedMessage = "Unable to generate AI response. Please try again.";

    private string? AiOverviewText =>
        Vehicle?.AiData.TryGetValue(AiType.Overview.ToString(), out var aiDataModel) == true
            ? aiDataModel.Content
            : string.Empty;

    private string? AiCommonIssuesText =>
        Vehicle?.AiData.TryGetValue(AiType.CommonIssues.ToString(), out var aiDataModel) == true
            ? aiDataModel.Content
            : string.Empty;

    private string? AiMotHistorySummaryText =>
        Vehicle?.AiData.TryGetValue(AiType.MotHistorySummary.ToString(), out var aiDataModel) == true
            ? aiDataModel.Content
            : string.Empty;

    private MarkupString? AiOverviewHtml =>
        string.IsNullOrWhiteSpace(AiOverviewText)
            ? null
            : (MarkupString)Markdig.Markdown.ToHtml(AiOverviewText);

    private MarkupString? AiCommonIssuesHtml =>
        string.IsNullOrWhiteSpace(AiCommonIssuesText)
            ? null
            : (MarkupString)Markdig.Markdown.ToHtml(AiCommonIssuesText);

    private MarkupString? AiMotHistorySummaryHtml =>
        string.IsNullOrWhiteSpace(AiMotHistorySummaryText)
            ? null
            : (MarkupString)Markdig.Markdown.ToHtml(AiMotHistorySummaryText);

    private async Task StartLookup(VehicleLookupType lookupType)
    {
        if (Vehicle?.Details?.RegistrationNumber != null)
        {
            await LookupState.StartLookupAsync(Vehicle.Details.RegistrationNumber, lookupType);
        }
    }

    private async Task OnCommonIssuesExpandedAsync(bool expanded)
    {
        if (Vehicle == null)
        {
            return;
        }

        if (!IsSearchingAiCommonIssues && !Vehicle.AiData.ContainsKey(AiType.CommonIssues.ToString()))
        {
            await StartLookup(VehicleLookupType.AiCommonIssues);
        }
    }

    private async Task OnMotHistoryExpandedAsync(bool expanded)
    {
        if (Vehicle == null)
        {
            return;
        }

        if (!IsSearchingMotHistory && !Vehicle.MotTests.Any())
        {
            await StartLookup(VehicleLookupType.MotHistory);
        }

        if (!IsSearchingAiMotHistorySummary && !Vehicle.AiData.ContainsKey(AiType.MotHistorySummary.ToString()))
        {
            await StartLookup(VehicleLookupType.AiMotHistorySummary);
        }
    }
}
