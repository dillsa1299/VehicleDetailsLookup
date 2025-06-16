using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;
using VehicleDetailsLookup.Shared.Models;
using VehicleDetailsLookup.Shared.Models.Enums;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails
{
    public partial class VehicleDetails
    {
        [Inject]
        private IVehicleLookupService VehicleLookupService { get; set; } = default!;

        [Inject]
        private IVehicleLookupEventsService VehicleLookupEventsService { get; set; } = default!;

        [Parameter]
        public VehicleModel? Vehicle { get; set; }

        private readonly string _placeholderImage = "images/placeholder-car.svg";
        private readonly string _aiFailedMessage = "Unable to generate AI response. Please try again.";
        private bool _isSearchingDetails;
        private bool _isSearchingImages;
        private bool _isSearchingAiOverview;
        private bool _isSearchingAiCommonIssues;
        private bool _isSearchingAiMotHistorySummary;
        private int _vehicleLookupCount;

        private string? AiOverviewText =>
            Vehicle?.AiData.FirstOrDefault(x => x.Type == VehicleAiType.Overview)?.Content ?? string.Empty;

        private string? AiCommonIssuesText =>
            Vehicle?.AiData.FirstOrDefault(x => x.Type == VehicleAiType.CommonIssues)?.Content ?? string.Empty;

        private string? AiMotHistorySummaryText =>
            Vehicle?.AiData.FirstOrDefault(x => x.Type == VehicleAiType.MotHistorySummary)?.Content ?? string.Empty;

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

        private void OnLookupStatusChanged(VehicleLookupType lookupType, bool lookupStarted, string registrationNumber)
        {
            switch (lookupType)
            {
                case VehicleLookupType.Details:
                    _isSearchingDetails = lookupStarted;
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
            if (Vehicle != null)
            {
                await VehicleLookupEventsService.NotifyStartVehicleLookup(Vehicle.RegistrationNumber, lookupType);
            }
        }

        private async Task OnCommonIssuesExpandedAsync(bool expanded)
        {
            if (!_isSearchingAiCommonIssues && Vehicle != null && !Vehicle.AiData.Any(d => d.Type == VehicleAiType.CommonIssues))
            {
                await StartLookup(VehicleLookupType.AiCommonIssues);
            }
        }

        private async Task OnMotHistoryExpandedAsync(bool expanded)
        {
            if (!_isSearchingAiMotHistorySummary && Vehicle != null && !Vehicle.AiData.Any(d => d.Type == VehicleAiType.MotHistorySummary))
            {
                await StartLookup(VehicleLookupType.AiMotHistorySummary);
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!String.IsNullOrEmpty(Vehicle?.RegistrationNumber))
            {
                _vehicleLookupCount = await VehicleLookupService.GetVehicleLookupCountAsync(Vehicle.RegistrationNumber);
            }
            else
            {
                _vehicleLookupCount = 0;
            }

            await base.OnParametersSetAsync();
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
