using System;
using Darker.Scheduler.Data.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;

namespace Darker.Scheduler.Data.Context;

public class SchedulerDbContext : DbContext
{
    public SchedulerDbContext(DbContextOptions<SchedulerDbContext> options) : base(options)
    {
    }

    public DbSet<Establishment> Establishments { get; set; } = null!;
    public DbSet<Schedule> Schedules { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Store Configuration and Details as JSON (string)
        modelBuilder.Entity<Establishment>()
            .Property(e => e.Type)
            .HasConversion(new EnumToStringConverter<EstablishmentType>());

        modelBuilder.Entity<Schedule>()
            .Property(s => s.Configuration)
            .HasColumnType("TEXT");

        modelBuilder.Entity<Order>()
            .Property(o => o.Details)
            .HasColumnType("TEXT");

        base.OnModelCreating(modelBuilder);
    }
}
