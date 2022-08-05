using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NamedRangeTestApp.DataAccess;
using NamedRangeTestApp.DataAccess.Base;
using NamedRangeTestApp.Services;
using NamedRangeTestApp.Services.Base;

namespace NamedRangeTestApp.Extensions;

internal static class ProgramExtensions
{
    internal static IServiceCollection InitApp(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<INamedRangeService, NamedRangeService>();

        services.AddTransient<INamedRangeExcelService, NamedRangeExcelService>();
        services.AddTransient<ITestExcelService, TestExcelService>();

        return services;
    }
}

