using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Services.VehicleLookup;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.Lookups.LookupCount
{
    public partial class LookupCount
    {
        [Parameter]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Inject]
        private IVehicleLookupService VehicleLookupService { get; set; } = default!;

        private int _lookupCount;
        private bool _historyVisible;

        private string LookupCountText => (_lookupCount == 0 ? "" : _lookupCount.ToString());

        private void OnLookupClicked() => _historyVisible = !_historyVisible;

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(RegistrationNumber))
                _lookupCount = await VehicleLookupService.GetVehicleLookupCountAsync(RegistrationNumber) ?? 0;

            await base.OnParametersSetAsync();
        }
    }
}
