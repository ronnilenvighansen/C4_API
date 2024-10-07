using System.Diagnostics;
using System.Reflection;
using Serilog;
using Serilog.Formatting.Json;

public static class AppExtension
{
    public static readonly string ServiceName = Assembly.GetCallingAssembly().GetName().Name ?? "Unknown";
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
    public static Serilog.ILogger Log => Serilog.Log.Logger;
    static AppExtension()
    {
        Serilog.Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .CreateLogger();
    }
}