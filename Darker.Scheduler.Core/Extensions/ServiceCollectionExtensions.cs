using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Darker.Scheduler.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureApp(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return services
            .ConfigureLogging(configuration);
    }

    private static IServiceCollection ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console(Serilog.Events.LogEventLevel.Verbose)
            .CreateLogger();
        return services;
    }
}
