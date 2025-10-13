using Darker.Scheduler.Data.Context;
using Darker.Scheduler.Data.Helpers;
using Darker.Scheduler.Data.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Darker.Scheduler.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment? environment = null)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        var configConnectionString = configuration.GetConnectionString("DefaultConnection");
        var isDevelopment = environment?.IsDevelopment() ?? false;
        var connectionString = DatabaseHelper.GetConnectionString(configConnectionString, isDevelopment);

        services.AddDbContext<SchedulerDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<DataSeedingService>();

        return services;
    }
}
