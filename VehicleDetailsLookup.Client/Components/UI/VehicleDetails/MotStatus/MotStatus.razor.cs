using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.MotStatus
{
    public partial class MotStatus
    {
        [Parameter]
        public IVehicleModel? Vehicle { get; set; }

        private string _statusText = string.Empty;
        private string _style = string.Empty;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            switch (Vehicle?.Details?.MotStatus)
            {
                case Shared.Models.Enums.MotStatus.Valid:
                    _statusText = "Valid";
                    _style = "background-color: var(--mud-palette-success);";
                    break;
                case Shared.Models.Enums.MotStatus.Invalid:
                    _statusText = "Invalid";
                    _style = "background-color: var(--mud-palette-error);";
                    break;
                case Shared.Models.Enums.MotStatus.NoDetails:
                    _statusText = "No details held by DVLA";
                    _style = "background-color: var(--mud-palette-gray-default);";
                    break;
                case Shared.Models.Enums.MotStatus.NoResults:
                    _statusText = "No results found";
                    _style = "background-color: var(--mud-palette-gray-default);";
                    break;
                default:
                    _statusText = "Unknown";
                    _style = "background-color: var(--mud-palette-gray-default);";
                    break;
            }
        }
    }
}
