using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Core.Extensions
{
    public static class ConfigurationExtensions
    {
        private const string _folderName = "Configurations";

        public static IConfigurationBuilder AddConfigurations(this IConfigurationBuilder builder, HostBuilderContext context)
        {
            var env = context.HostingEnvironment;
            builder.SetBasePath(env.ContentRootPath);
            builder.AddJsonFile(Path.Combine(AppContext.BaseDirectory, _folderName, "infrastructure.json"), optional: true, reloadOnChange: true)
                   .AddJsonFile(Path.Combine(AppContext.BaseDirectory, _folderName, $"infrastructure.{env.EnvironmentName}.json"), optional: true, reloadOnChange: true)
                   .AddJsonFile(Path.Combine(AppContext.BaseDirectory, _folderName, "appsettings.json"), optional: false, reloadOnChange: true)
                   .AddJsonFile(Path.Combine(AppContext.BaseDirectory, _folderName, $"appsettings.{env.EnvironmentName}.json"), optional: true, reloadOnChange: true);

            builder.AddEnvironmentVariables(prefix: "XL_");

            return builder;
        }
    }
}
