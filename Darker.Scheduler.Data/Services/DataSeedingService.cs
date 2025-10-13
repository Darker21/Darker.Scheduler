using Darker.Scheduler.Data.Context;
using Microsoft.Extensions.Logging;

namespace Darker.Scheduler.Data.Services;

public class DataSeedingService
{
    private SchedulerDbContext _context;
    private ILogger<DataSeedingService> _logger;

    public DataSeedingService(SchedulerDbContext context, ILogger<DataSeedingService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SeedAsync()
    {
        try
        {
            _logger.LogInformation("Starting database seeding...");

            // Ensure database is created
            await _context.Database.EnsureCreatedAsync();

            await SeedEstablishmentsAsync();
            await _context.SaveChangesAsync();

            await SeedSchedulesAsync();

            await _context.SaveChangesAsync();
            _logger.LogInformation("Database seeding completed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task SeedSchedulesAsync()
    {
        if (_context.Schedules.Any())
        {
            return;
        }

        _logger.LogInformation("Seeding schedules...");

        var establishments = _context.Establishments.ToList();
        if (!establishments.Any())
        {
            _logger.LogWarning("No establishments found. Skipping schedule seeding.");
            return;
        }

        var schedules = new[]
        {
            new Models.Schedule
            {
                EstablishmentId = establishments.First().Id,
                CronExpression = "*/60 * * * *", // Every day, every hour
                MaxDaysInAdvance = 30,
                Configuration = "{\"OpenHours\":\"9AM-9PM\",\"Days\":\"Mon-Sun\"}"
            },
            new Models.Schedule
            {
                EstablishmentId = establishments.Last().Id,
                CronExpression = "0 12 * * 1-5", // Weekdays at noon
                MaxDaysInAdvance = 15,
                Configuration = "{\"OpenHours\":\"11AM-11PM\",\"Days\":\"Mon-Sun\"}"
            }
        };

        await _context.Schedules.AddRangeAsync(schedules);
        _logger.LogInformation("Seeded {Count} schedules.", schedules.Length);
    }

    private async Task SeedEstablishmentsAsync()
    {
        if (_context.Establishments.Any())
        {
            return;
        }

        _logger.LogInformation("Seeding establishments...");

        var establishments = new[]
        {
            new Models.Establishment { Name = "The Tipsy Tavern", Type = Models.EstablishmentType.Bar },
            new Models.Establishment { Name = "Gourmet Bistro", Type = Models.EstablishmentType.Restaurant }
        };

        await _context.Establishments.AddRangeAsync(establishments);
        _logger.LogInformation("Seeded {Count} establishments.", establishments.Length);
    }
}
