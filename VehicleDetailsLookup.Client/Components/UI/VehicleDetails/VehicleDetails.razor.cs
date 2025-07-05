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

        private readonly string _placeholderImage = "images/placeholder-car.svg";
        private readonly string _aiFailedMessage = "Unable to generate AI response. Please try again.";
        private bool _isSearchingDetails;
        private bool _isSearchingMotHistory;
        private bool _isSearchingImages;
        private bool _isSearchingAiOverview;
        private bool _isSearchingAiCommonIssues;
        private bool _isSearchingAiMotHistorySummary;
        private bool _clarksonEasterEggEnabled;

        private string? AiOverviewText =>
            _clarksonEasterEggEnabled
                ? (Vehicle?.AiData.TryGetValue(AiType.ClarksonOverview, out var aiDataModel) == true
                    ? aiDataModel.Content
                    : string.Empty)
                : (Vehicle?.AiData.TryGetValue(AiType.Overview, out aiDataModel) == true
                    ? aiDataModel.Content
                    : string.Empty);

        private string? AiCommonIssuesText =>
            _clarksonEasterEggEnabled
                ? (Vehicle?.AiData.TryGetValue(AiType.ClarksonCommonIssues, out var aiDataModel) == true
                    ? aiDataModel.Content
                    : string.Empty)
                : (Vehicle?.AiData.TryGetValue(AiType.CommonIssues, out aiDataModel) == true)
                    ? aiDataModel.Content
                    : string.Empty;

        private string? AiMotHistorySummaryText =>
            _clarksonEasterEggEnabled
                ? (Vehicle?.AiData.TryGetValue(AiType.ClarksonMotHistorySummary, out var aiDataModel) == true
                    ? aiDataModel.Content
                    : string.Empty)
                : (Vehicle?.AiData.TryGetValue(AiType.MotHistorySummary, out aiDataModel) == true)
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

        private string? AiOverviewTitle =>
            _clarksonEasterEggEnabled
                ? "Clarkson's Take"
                : "AI Overview";

        private string? CommonIssuesTitle =>
            _clarksonEasterEggEnabled
                ? "Clarkson's Common Gripes"
                : "Common Issues";

        private string? AiMotHistorySummaryTitle =>
            _clarksonEasterEggEnabled
                ? "Clarkson's MOT Rant"
                : "AI Summary";

        private void OnLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted, string registrationNumber)
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
                case VehicleLookupType.AiClarksonOverview:
                    _isSearchingAiOverview = lookupStarted;
                    break;
                case VehicleLookupType.AiCommonIssues:
                case VehicleLookupType.AiClarksonCommonIssues:
                    _isSearchingAiCommonIssues = lookupStarted;
                    break;
                case VehicleLookupType.AiMotHistorySummary:
                case VehicleLookupType.AiClarksonMotHistorySummary:
                    _isSearchingAiMotHistorySummary = lookupStarted;
                    break;
            }

            StateHasChanged();
        }

        private async Task StartLookup(VehicleLookupType lookupType)
        {
            if (Vehicle?.Details?.RegistrationNumber != null)
            {
                await VehicleLookupEventsService.NotifyStartVehicleLookup(Vehicle.Details.RegistrationNumber, lookupType);
            }
        }

        private async Task OnCommonIssuesExpandedAsync(bool expanded)
        {
            if (!_isSearchingAiCommonIssues && Vehicle != null && !Vehicle.AiData.ContainsKey(AiType.CommonIssues) && !_clarksonEasterEggEnabled)
            {
                await StartLookup(VehicleLookupType.AiCommonIssues);
            }
        }

        private async Task OnMotHistoryExpandedAsync(bool expanded)
        {
            if (!_isSearchingMotHistory && Vehicle != null && !Vehicle.MotTests.Any())
            {
                await StartLookup(VehicleLookupType.MotHistory);
            }

            if (!_isSearchingAiMotHistorySummary && Vehicle != null && !Vehicle.AiData.ContainsKey(AiType.MotHistorySummary) && !_clarksonEasterEggEnabled)
            {
                await StartLookup(VehicleLookupType.AiMotHistorySummary);
            }
        }

        private void OnEasterEggActivated(bool activated)
        {
            _clarksonEasterEggEnabled = activated;
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            VehicleLookupEventsService.OnLookupStatusChanged += OnLookupStatusChanged;
            VehicleLookupEventsService.OnEasterEggActivated += OnEasterEggActivated;
            base.OnInitialized();
        }

        public void Dispose()
        {
            VehicleLookupEventsService.OnLookupStatusChanged -= OnLookupStatusChanged;
            VehicleLookupEventsService.OnEasterEggActivated -= OnEasterEggActivated;
        }
    }
}
