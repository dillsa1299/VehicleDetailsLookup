using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Shared.Models.Mot;
using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.MotHistory
{
    public partial class MotHistory
    {
        [Parameter]
        public VehicleModel Vehicle { get; set; } = default!;
    }
}
