using Core.Extensions;
using Core.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NamedRangeTestApp.Extensions;
using NLog;
using NLog.Web;
using System;

LogManager.Setup(builder => builder.LoadConfiguration(NLogConfigHelpers.GetNLogWebConfig()));
var logger = LogManager.GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;

    builder.Host.ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddConfigurations(context);
    });

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();
    builder.Services.InitApp(configuration);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    logger.Info($"Starting NamedRangeTestApp");

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "NamedRangeTestApp stopped because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}
