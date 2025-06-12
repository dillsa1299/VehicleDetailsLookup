using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Shared.Models;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.MotHistory
{
    public partial class MotHistory
    {
        [Parameter]
        public VehicleModel? Vehicle { get; set; }
    }
}
