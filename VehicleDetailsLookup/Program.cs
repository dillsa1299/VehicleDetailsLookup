using MudBlazor.Services;
using VehicleDetailsLookup.Components;
using VehicleDetailsLookup.Services.VehicleDataService;
using VehicleDetailsLookup.Services.VehicleDetailsService;
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
builder.Services.AddSingleton<IVehicleDetailsService, VehicleDetailsService>();
builder.Services.AddSingleton<IVehicleMapperService, VehicleMapperService>();


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
