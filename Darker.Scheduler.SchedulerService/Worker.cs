using Darker.Scheduler.Data.Context;
using Darker.Scheduler.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NCrontab;

namespace Darker.Scheduler.SchedulerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Scheduler Worker Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessScheduledJobs();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in scheduler service");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        _logger.LogInformation("Scheduler Worker Service is stopping");
    }

    private async Task ProcessScheduledJobs()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SchedulerDbContext>();

        var activeSchedules = await context.Schedules
            .Where(s => s.IsActive)
            .Include(s => s.Establishment)
            .ToListAsync();

        var now = DateTime.UtcNow;

        foreach (var schedule in activeSchedules)
        {
            try
            {
                if (ShouldExecuteSchedule(schedule, now))
                {
                    await ExecuteSchedule(context, schedule, now);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing schedule {ScheduleId} for {EstablishmentName}", 
                    schedule.Id, schedule.Establishment.Name);
            }
        }
    }

    private bool ShouldExecuteSchedule(Schedule schedule, DateTime now)
    {
        try
        {
            var cron = CrontabSchedule.Parse(schedule.CronExpression);
            var nextOccurrence = cron.GetNextOccurrence(now.AddMinutes(-1));
            return Math.Abs((nextOccurrence - now).TotalMinutes) < 1;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Invalid CRON expression for schedule {ScheduleId}: {CronExpression}", 
                schedule.Id, schedule.CronExpression);
            return false;
        }
    }

    private async Task ExecuteSchedule(SchedulerDbContext context, Schedule schedule, DateTime now)
    {
        var targetDate = CalculateTargetDate(now, schedule.MaxDaysInAdvance);
        
        _logger.LogInformation("Executing schedule {ScheduleId} for {EstablishmentName} targeting {TargetDate:yyyy-MM-dd}",
            schedule.Id, schedule.Establishment.Name, targetDate);

        var order = new Order
        {
            ScheduleId = schedule.Id,
            CreatedAt = now,
            Details = $"{{\"targetDate\":\"{targetDate:yyyy-MM-dd}\",\"configuration\":{schedule.Configuration}}}"
        };

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        _logger.LogInformation("Created order {OrderId} for schedule {ScheduleId}", order.Id, schedule.Id);
    }

    private static DateTime CalculateTargetDate(DateTime now, int maxDaysInAdvance)
    {
        var random = new Random();
        var daysToAdd = random.Next(1, maxDaysInAdvance + 1);
        return now.Date.AddDays(daysToAdd);
    }
}
