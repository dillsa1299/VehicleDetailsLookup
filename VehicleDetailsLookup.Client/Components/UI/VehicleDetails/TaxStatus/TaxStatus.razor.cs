using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.TaxStatus
{
    public partial class TaxStatus
    {
        [Parameter]
        public VehicleModel? Vehicle { get; set; }

        private string _style = string.Empty;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            // Set _style based on the MOT status
            _style = (Vehicle?.TaxStatus?.ToLowerInvariant()) switch
            {
                "taxed" => "background-color: var(--mud-palette-info);",
                "untaxed" => "background-color: var(--mud-palette-error);",
                "sorn" => "background-color: var(--mud-palette-warning);",
                _ => "background-color: var(--mud-palette-gray-default);",
            };
        }
    }
}
