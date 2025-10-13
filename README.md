# Darker.Scheduler (WIP)

A modular solution for automated restaurant/bar stock scheduling, management, and observability. Built with ASP.NET Core, Blazor, WinForms, Worker Services, and Entity Framework Core (SQLite).

## The vision

I have a few automations in different stacks and want one solution to "rule them all". This is a template for a private project which shows how I can have a web-ui, scheduler and a standalone exe. My initial use case is for a playwright automation script.

The end goal is to have a solution which supports trusted DLL's/Exe's with a notification system built in to support different Systems

## Solution Structure

- **Darker.Scheduler.Blaze**: Blazor Server web app for CRUD management of establishments, schedules, and orders.
- **Darker.Scheduler.ConsoleApp**: Console application for executing automated stock processes.
- **Darker.Scheduler.Core**: Core business logic and service abstractions.
- **Darker.Scheduler.TrayApp**: WinForms tray application for controlling the web server and scheduler service processes.
- **Darker.Scheduler.SchedulerService**: Worker service for background scheduling and process orchestration.
- **Darker.Scheduler.Data**: Entity Framework Core data access layer using SQLite.

## Data Model

- **Establishment**: Represents a bar or restaurant. Fields: `Id`, `Name`, `Type` ("Bar"/"Restaurant"), navigation to schedules.
- **Schedule**: Defines a CRON-based schedule for an establishment. Fields: `Id`, `Name`, `EstablishmentId`, `CronExpression`, `MaxDaysInAdvance`, `Configuration` (JSON), `IsActive` (soft delete), navigation to orders.
- **Order**: Stores order events for observability. Fields: `Id`, `ScheduleId`, `CreatedAt`, `Details` (JSON/text).

## Features

- **Blazor CRUD UI**: Manage establishments, schedules, and orders with modals for create/edit, table views, and soft delete (IsActive toggle).
- **Tray App**: Start/stop web and scheduler services via tray icon. Configurable via `appsettings.json`.
- **Background Scheduling**: Worker service executes scheduled jobs using CRON expressions and max days logic.
- **SQLite Persistence**: All data stored in a local SQLite database. Configuration and details fields support complex JSON payloads.
- **Extensible Core**: Business logic and service interfaces in the Core project for easy extension and testing.

## Getting Started

1. **Clone the repository**
2. **Restore dependencies**: `dotnet restore`
3. **Build the solution**: `dotnet build`
4. **Run the web app**: `dotnet run --project Darker.Scheduler.Blaze`
5. **Run the tray app**: `dotnet run --project Darker.Scheduler.TrayApp`
6. **Run the scheduler service**: `dotnet run --project Darker.Scheduler.SchedulerService`

## Configuration

- **appsettings.json** in each project controls paths, connection strings, and other settings.
- **TrayApp**: Configure executable paths for web and scheduler services.
- **Data**: Database location is set via connection string in DI setup.

## Development Notes

- Uses Entity Framework Core migrations for schema management.
- All CRUD pages use async EF operations and modals for UX.
- Soft delete is implemented via `IsActive` on schedules.
- Orders are linked to schedules for full observability.

## Extending

- Add new entity types by creating models and updating `SchedulerDbContext`.
- Extend business logic in the Core project.
- Add new UI pages in Blaze for additional features.

## License

MIT
