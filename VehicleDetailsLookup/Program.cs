using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using VehicleDetailsLookup.Client.Services.VehicleLookup;
using VehicleDetailsLookup.Client.Services.VehicleLookupEvents;
using VehicleDetailsLookup.Components;
using VehicleDetailsLookup.Models.Database;
using VehicleDetailsLookup.Services.Api.Gemini;
using VehicleDetailsLookup.Services.Api.GoogleImage;
using VehicleDetailsLookup.Services.Vehicle.Details;
using VehicleDetailsLookup.Services.Vehicle.AiData;
using VehicleDetailsLookup.Services.Vehicle.Images;
using VehicleDetailsLookup.Services.Api.Ves;
using VehicleDetailsLookup.Services.Api.Mot;
using VehicleDetailsLookup.Services.Vehicle.Mot;
using VehicleDetailsLookup.Services.Mappers.ApiDatabase;
using VehicleDetailsLookup.Services.Mappers.DatabaseFrontend;
using VehicleDetailsLookup.Repositories.AiData;
using VehicleDetailsLookup.Repositories.Details;
using VehicleDetailsLookup.Repositories.Image;
using VehicleDetailsLookup.Repositories.Mot;
using VehicleDetailsLookup.Repositories.Lookup;

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

builder.Services.AddScoped<IVesService, VesService>();
builder.Services.AddScoped<IMotService, MotService>();
builder.Services.AddScoped<IGoogleImageService, GoogleImageService>();
builder.Services.AddScoped<IGeminiService, GeminiService>();

builder.Services.AddScoped<IApiDatabaseMapperService, ApiDatabaseMapperService>();
builder.Services.AddScoped<IDatabaseFrontendMapperService, DatabaseFrontendMapperService>();

builder.Services.AddScoped<IVehicleDetailsService, VehicleDetailsService>();
builder.Services.AddScoped<IVehicleMotService, VehicleMotService>();
builder.Services.AddScoped<IVehicleImageService, VehicleImageService>();
builder.Services.AddScoped<IVehicleAiDataService, VehicleAiDataService>();

builder.Services.AddScoped<IVehicleLookupService, VehicleLookupService>();
builder.Services.AddScoped<IVehicleLookupEventsService, VehicleLookupEventsService>();

// Register repositories
builder.Services.AddScoped<IDetailsRepository, DetailsRepository>();
builder.Services.AddScoped<IMotRepository, MotRepository>();
builder.Services.AddScoped<IAiDataRepository, AiDataRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<ILookupRepository, LookupRepository>();

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
