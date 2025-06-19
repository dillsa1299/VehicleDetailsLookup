using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Components.Enums;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;
using VehicleDetailsLookup.Shared.Models.VehicleLookup;

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

        private IEnumerable<VehicleLookupModel> _recentLookups = [];
        private bool _loading = true;

        private async Task HandleLookupClick(string registrationNumber)
        {
            await VehicleLookupEventsService.NotifyStartVehicleLookup(registrationNumber, VehicleLookupType.Details);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (OperatingSystem.IsBrowser())
            {
                _loading = true;
                _recentLookups = await VehicleLookupService.GetRecentVehicleLookupsAsync();
                _loading = false;
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
