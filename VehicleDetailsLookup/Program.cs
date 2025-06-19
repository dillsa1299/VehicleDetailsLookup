using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;
using VehicleDetailsLookup.Components;
using VehicleDetailsLookup.Models.Database;
using VehicleDetailsLookup.Services.AiSearch;
using VehicleDetailsLookup.Services.ImageSearch;
using VehicleDetailsLookup.Services.VehicleAi;
using VehicleDetailsLookup.Services.VehicleData;
using VehicleDetailsLookup.Services.VehicleDetails;
using VehicleDetailsLookup.Services.VehicleImages;
using VehicleDetailsLookup.Services.VehicleMapper;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register application services
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IVehicleDataService, VehicleDataService>();
builder.Services.AddSingleton<IImageSearchService, ImageSearchService>();
builder.Services.AddSingleton<IAiSearchService, AiSearchService>();

builder.Services.AddSingleton<IVehicleMapperService, VehicleMapperService>();
builder.Services.AddSingleton<IVehicleDetailsService, VehicleDetailsService>();
builder.Services.AddSingleton<IVehicleImagesService, VehicleImagesService>();
builder.Services.AddSingleton<IVehicleAiService, VehicleAiService>();

builder.Services.AddScoped<IVehicleLookupService, VehicleLookupService>();
builder.Services.AddScoped<IVehicleLookupEventsService, VehicleLookupEventsService>();

// Register DbContext with SQLite
builder.Services.AddDbContext<VehicleDbContext>(options =>
    options.UseSqlite("Data Source=vehicledata.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();

    // Enable Swagger in development
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(VehicleDetailsLookup.Client._Imports).Assembly);

app.Run();
