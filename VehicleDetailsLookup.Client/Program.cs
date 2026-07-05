using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Client.State;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<VehicleLookupState>();
builder.Services.AddScoped<IVehicleLookupService, VehicleLookupService>();

await builder.Build().RunAsync();
