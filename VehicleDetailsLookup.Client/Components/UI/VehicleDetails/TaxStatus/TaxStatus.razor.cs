using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Client.Styling;
using VehicleDetailsLookup.Shared.Models.Details;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.TaxStatus
{
    public partial class TaxStatus
    {
        [Parameter]
        public DetailsModel? Details { get; set; }

        private string _statusText = string.Empty;
        private string _statusClass = StatusCardCss.Combine(StatusCardCss.Base, StatusCardCss.Neutral);

        private string ExpiryDateText =>
            Details?.TaxDueDate is DateOnly dueDate
                ? $"{(dueDate < DateOnly.FromDateTime(DateTime.Today) ? "Expired:" : "Expires:")} {dueDate:dd/MM/yyyy}"
                : string.Empty;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            switch (Details?.TaxStatus)
            {
                case Shared.Models.Enums.TaxStatus.Taxed:
                    _statusText = "Taxed";
                    _statusClass = StatusCardCss.Combine(StatusCardCss.Base, StatusCardCss.Info);
                    break;
                case Shared.Models.Enums.TaxStatus.Untaxed:
                    _statusText = "Untaxed";
                    _statusClass = StatusCardCss.Combine(StatusCardCss.Base, StatusCardCss.Error);
                    break;
                case Shared.Models.Enums.TaxStatus.Sorn:
                    _statusText = "SORN";
                    _statusClass = StatusCardCss.Combine(StatusCardCss.Base, StatusCardCss.Warning);
                    break;
                default:
                    _statusText = "Unknown";
                    _statusClass = StatusCardCss.Combine(StatusCardCss.Base, StatusCardCss.Neutral);
                    break;
            }
        }
    }
}
