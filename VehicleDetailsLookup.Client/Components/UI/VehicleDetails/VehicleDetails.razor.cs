using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;
using VehicleDetailsLookup.Shared.Models.Enums;
using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails
{
    public partial class VehicleDetails
    {
        [Inject]
        private IVehicleLookupEventsService VehicleLookupEventsService { get; set; } = default!;

        [Parameter]
        public VehicleModel? Vehicle { get; set; }

        private const string _placeholderImage = "images/placeholder-car.svg";
        private const string _aiFailedMessage = "Unable to generate AI response. Please try again.";
        private bool _isSearchingDetails;
        private bool _isSearchingMotHistory;
        private bool _isSearchingImages;
        private bool _isSearchingAiOverview;
        private bool _isSearchingAiCommonIssues;
        private bool _isSearchingAiMotHistorySummary;

        private string? AiOverviewText =>
            (Vehicle?.AiData.TryGetValue(AiType.Overview.ToString(), out var aiDataModel) == true
                ? aiDataModel.Content
                : string.Empty);

        private string? AiCommonIssuesText =>
            (Vehicle?.AiData.TryGetValue(AiType.CommonIssues.ToString(), out var aiDataModel) == true)
                ? aiDataModel.Content
                : string.Empty;

        private string? AiMotHistorySummaryText =>
            (Vehicle?.AiData.TryGetValue(AiType.MotHistorySummary.ToString(), out var aiDataModel) == true)
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

        private void OnLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted, string registrationNumber, string metaData)
        {
            switch (lookupType)
            {
                case VehicleLookupType.Details:
                    _isSearchingDetails = lookupStarted;
                    break;
                case VehicleLookupType.MotHistory:
                    _isSearchingMotHistory = lookupStarted;
                    break;
                case VehicleLookupType.Images:
                    _isSearchingImages = lookupStarted;
                    break;
                case VehicleLookupType.AiOverview:
                    _isSearchingAiOverview = lookupStarted;
                    break;
                case VehicleLookupType.AiCommonIssues:
                    _isSearchingAiCommonIssues = lookupStarted;
                    break;
                case VehicleLookupType.AiMotHistorySummary:
                    _isSearchingAiMotHistorySummary = lookupStarted;
                    break;
            }

            StateHasChanged();
        }

        private async Task StartLookup(VehicleLookupType lookupType)
        {
            if (Vehicle?.Details?.RegistrationNumber != null)
                await VehicleLookupEventsService.NotifyStartVehicleLookup(Vehicle.Details.RegistrationNumber, lookupType);
        }

        private async Task OnCommonIssuesExpandedAsync(bool expanded)
        {
            if (Vehicle == null)
                return;

            // Start lookup for AI Common Issues if not already searching and no data already loaded
            if (!_isSearchingAiCommonIssues && !Vehicle.AiData.ContainsKey(AiType.CommonIssues.ToString()))
                await StartLookup(VehicleLookupType.AiCommonIssues);
        }

        private async Task OnMotHistoryExpandedAsync(bool expanded)
        {
            if (Vehicle == null)
                return;

            // Start lookup for MOT History if not already searching and no data already loaded
            if (!_isSearchingMotHistory && !Vehicle.MotTests.Any())
                await StartLookup(VehicleLookupType.MotHistory);

            // Start lookup for AI MOT History Summary if not already searching and no data already loaded
            if (!_isSearchingAiMotHistorySummary && !Vehicle.AiData.ContainsKey(AiType.MotHistorySummary.ToString()))
                await StartLookup(VehicleLookupType.AiMotHistorySummary);
        }

        protected override void OnInitialized()
        {
            VehicleLookupEventsService.OnLookupStatusChanged += OnLookupStatusChanged;
            base.OnInitialized();
        }

        public void Dispose()
        {
            VehicleLookupEventsService.OnLookupStatusChanged -= OnLookupStatusChanged;
        }
    }
}
