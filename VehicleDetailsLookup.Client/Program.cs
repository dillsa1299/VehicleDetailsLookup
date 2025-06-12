using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IVehicleLookupService, VehicleLookupService>();
builder.Services.AddScoped<IVehicleLookupEventsService, VehicleLookupEventsService>();

await builder.Build().RunAsync();
