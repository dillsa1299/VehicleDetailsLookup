﻿using Microsoft.AspNetCore.Components;
using VehicleDetailsLookup.Shared.Models.Vehicle;

namespace VehicleDetailsLookup.Client.Components.UI.VehicleDetails.MotHistory
{
    public partial class MotHistory
    {
        [Parameter]
        public IVehicleModel? Vehicle { get; set; }
    }
}
