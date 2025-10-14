using MudBlazor.Services;
using Darker.Scheduler.Muddy.Components;
using Darker.Scheduler.Core.Extensions;
using Darker.Scheduler.Data.Extensions;
using Darker.Scheduler.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

builder.Services.ConfigureApp(builder.Configuration);
builder.Services.AddDataServices(builder.Configuration, builder.Environment);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dataSeedingService = services.GetRequiredService<DataSeedingService>();
    await dataSeedingService.SeedAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
