using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;

namespace Core.Logging;

public static class NLogConfigHelpers
{
    private const string _folderName = "Configurations";

    public static NLogLoggingConfiguration GetNLogWebConfig(params string[] pathItems)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return GetNLogConfig(env, pathItems);
    }

    public static NLogLoggingConfiguration GetNLogConfig(params string[] pathItems)
    {
        var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        return GetNLogConfig(env, pathItems);
    }

    private static NLogLoggingConfiguration GetNLogConfig(string env, string[] pathItems)
    {
        var folder = pathItems.Any()
            ? Path.Combine(pathItems)
            : _folderName;

        var builder = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(folder, "nlog.json"), optional: false, reloadOnChange: false)
            .AddJsonFile(Path.Combine(folder, $"nlog.{env}.json"), optional: true, reloadOnChange: false)
            .AddJsonFile(Path.Combine(folder, "appsettings.json"), optional: false, reloadOnChange: false)
            .AddJsonFile(Path.Combine(folder, $"appsettings.{env}.json"), optional: true, reloadOnChange: false);

        builder.AddEnvironmentVariables(prefix: "XL_");

        var config = builder.Build();
        var nlogConfig = new NLogLoggingConfiguration(config.GetSection("NLog"));

        return nlogConfig;
    }
}
