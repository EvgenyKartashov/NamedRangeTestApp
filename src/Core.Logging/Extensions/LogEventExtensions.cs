using Core.Logging.Fluent;
using Microsoft.Extensions.Logging;


namespace Core.Logging.Extensions;

public static class LogEventExtensions
{
    //------------------------------------------Fluent---------------------------------------------//
    public static LogBuilder Log(this ILogger logger, LogLevel logLevel, string message, params object[] args)
    {
        var builder = new LogBuilder(logger, logLevel, message, args);
        return builder;
    }

    public static LogBuilder Trace(this ILogger logger, string message, params object[] args)
    {
        var builder = new LogBuilder(logger, LogLevel.Trace, message, args);
        return builder;
    }

    public static LogBuilder Debug(this ILogger logger, string message, params object[] args)
    {
        var builder = new LogBuilder(logger, LogLevel.Debug, message, args);
        return builder;
    }

    public static LogBuilder Info(this ILogger logger, string message, params object[] args)
    {
        var builder = new LogBuilder(logger, LogLevel.Information, message, args);
        return builder;
    }

    public static LogBuilder Warn(this ILogger logger, string message, params object[] args)
    {
        var builder = new LogBuilder(logger, LogLevel.Warning, message, args);
        return builder;
    }

    public static LogBuilder Error(this ILogger logger, string message, params object[] args)
    {
        var builder = new LogBuilder(logger, LogLevel.Error, message, args);
        return builder;
    }

    public static LogBuilder Critical(this ILogger logger, string message, params object[] args)
    {
        var builder = new LogBuilder(logger, LogLevel.Critical, message, args);
        return builder;
    }

    public static IDisposable AddRequestIdScope(this ILogger logger, Guid? requestId, Guid? uid = null, Guid? operatorId = null)
    {
        var kvList = new List<KeyValuePair<string, object>>();

        if (requestId.HasValue)
            kvList.Add(new KeyValuePair<string, object>("RequestId", requestId));

        if (uid.HasValue)
            kvList.Add(new KeyValuePair<string, object>("Uid", uid));

        if (operatorId.HasValue)
            kvList.Add(new KeyValuePair<string, object>("OperatorId", operatorId));

        return logger.BeginScope(kvList);
    }

    public static IDisposable AddTitleTypeScope(this ILogger logger, string titleType)
    {
        var kvList = new List<KeyValuePair<string, object>> { new KeyValuePair<string, object>("TitleType", titleType) }; ;

        return logger.BeginScope(kvList);
    }
}
