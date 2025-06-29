using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Helpers;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;
using VehicleDetailsLookup.Shared.Models.Lookup;

namespace VehicleDetailsLookup.Client.Components.UI.RecentLookups
{
    public partial class RecentLookups
    {
        [Inject]
        IVehicleLookupService VehicleLookupService { get; set; } = default!;

        [Inject]
        IVehicleLookupEventsService VehicleLookupEventsService { get; set; } = default!;

        [Parameter]
        public bool IsHidden { get; set; }

        private IEnumerable<ILookupModel> _recentLookups = [];
        private bool _loading = true;

        private async Task HandleLookupClick(string registrationNumber)
        {
            await VehicleLookupEventsService.NotifyStartVehicleLookup(registrationNumber, VehicleLookupType.Details);
        }

        private static string BuildVehicleDetails(ILookupModel lookup)
        {
            return $"{lookup.VehicleDetails.YearOfManufacture} {lookup.VehicleDetails.Make} {lookup.VehicleDetails.Model}";
        }

        private static string GetTimeSpan(DateTime dateTime)
            => TimeSpanHelper.GetTimeSpan(dateTime);

        protected override async Task OnParametersSetAsync()
        {
            if (!IsHidden && OperatingSystem.IsBrowser())
            {
                _loading = true;
                _recentLookups = await VehicleLookupService.GetRecentVehicleLookupsAsync() ?? [];
                _loading = false;
                StateHasChanged();
            }

            await base.OnParametersSetAsync();
        }
    }
}
