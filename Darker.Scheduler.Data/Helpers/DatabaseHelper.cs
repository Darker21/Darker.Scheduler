namespace Darker.Scheduler.Data.Helpers;

public static class DatabaseHelper
{
    public static string GetConnectionString(string? configConnectionString, bool isDevelopment)
    {
        if (!string.IsNullOrWhiteSpace(configConnectionString))
        {
            return configConnectionString;
        }

        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var folderPath = Path.Combine(appDataPath, "Darker", "Scheduler");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var dbFileName = isDevelopment ? "scheduler_dev.db" : "scheduler.db";
        var dbFilePath = Path.Combine(folderPath, dbFileName);
        return $"Data Source={dbFilePath}";
    }
}
