using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Shared.Helpers;
using VehicleDetailsLookup.Shared.Models.Lookup;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.Lookups.LookupHistory
{
    public partial class LookupHistory
    {
        [Parameter]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Inject]
        private IVehicleLookupService VehicleLookupService { get; set; } = default!;

        private IEnumerable<LookupModel> _lookups = [];

        private static string GetTimeSpan(DateTime dateTime)
            => TimeSpanHelper.GetTimeSpan(dateTime, false);

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(RegistrationNumber))
                _lookups = await VehicleLookupService.GetRecentVehicleLookupsAsync(RegistrationNumber) ?? [];

            await base.OnParametersSetAsync();
        }
    }
}
