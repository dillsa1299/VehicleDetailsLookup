using MudBlazor.Services;
using VehicleDetailsLookup.Components;
using VehicleDetailsLookup.Services.AiSearchService;
using VehicleDetailsLookup.Services.ImageSearchService;
using VehicleDetailsLookup.Services.VehicleAiService;
using VehicleDetailsLookup.Services.VehicleDataService;
using VehicleDetailsLookup.Services.VehicleDetailsService;
using VehicleDetailsLookup.Services.VehicleImagesService;
using VehicleDetailsLookup.Services.VehicleMapper;
using VehicleDetailsLookup.Services.VehicleMapperService;

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
