
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Darker.Scheduler.TrayApp;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new TrayApplicationContext());
    }
}

public class TrayApplicationContext : ApplicationContext
{
    private NotifyIcon trayIcon;
    private Process? _webProcess;
    private Process? _schedulerProcess;
    private readonly IConfiguration _configuration;

    public TrayApplicationContext()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Application.StartupPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Initialize Tray Icon
        trayIcon = new NotifyIcon()
        {
            Icon = System.Drawing.SystemIcons.Application,
            Text = "Darker Scheduler",
            ContextMenuStrip = CreateContextMenu(),
            Visible = true
        };
    }

    private ContextMenuStrip CreateContextMenu()
    {
        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Open Dashboard", null, (_, _) => { });
        contextMenu.Items.Add("-");
        contextMenu.Items.Add("Exit", null, (_,_) => Exit());
        return contextMenu;
    }

    void Exit()
    {
        _webProcess?.Kill();
        _schedulerProcess?.Kill();
        trayIcon.Visible = false;

        Application.Exit();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            trayIcon.Dispose();
            _webProcess?.Dispose();
            _schedulerProcess?.Dispose();
        }

        base.Dispose(disposing);
    }
}