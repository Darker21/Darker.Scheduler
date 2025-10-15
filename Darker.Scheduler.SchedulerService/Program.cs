using Darker.Scheduler.Core.Extensions;
using Darker.Scheduler.Data.Context;
using Darker.Scheduler.Data.Extensions;
using Darker.Scheduler.SchedulerService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.ConfigureApp(builder.Configuration);
builder.Services.AddDataServices(builder.Configuration, builder.Environment);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SchedulerDbContext>();
    context.Database.EnsureCreated();
}

await host.RunAsync();
