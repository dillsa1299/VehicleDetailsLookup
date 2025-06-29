using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Shared.Models.Details;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.TaxStatus
{
    public partial class TaxStatus
    {
        [Parameter]
        public IDetailsModel? Details { get; set; }

        private string _statusText = string.Empty;
        private string _style = string.Empty;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            switch (Details?.TaxStatus)
            {
                case Shared.Models.Enums.TaxStatus.Taxed:
                    _statusText = "Taxed";
                    _style = "background-color: var(--mud-palette-info);";
                    break;
                case Shared.Models.Enums.TaxStatus.Untaxed:
                    _statusText = "Untaxed";
                    _style = "background-color: var(--mud-palette-error);";
                    break;
                case Shared.Models.Enums.TaxStatus.Sorn:
                    _statusText = "SORN";
                    _style = "background-color: var(--mud-palette-warning);";
                    break;
                default:
                    _statusText = "Unknown";
                    _style = "background-color: var(--mud-palette-gray-default);";
                    break;
            }
        }
    }
}
