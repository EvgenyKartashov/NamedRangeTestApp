using Microsoft.Extensions.Logging;

namespace Core.Logging.Fluent;

public class LogBuilder
{
    private readonly LogEvent _logEvent;
    private readonly ILogger _logger;

    private Exception _exception;
    private LogLevel _level;
    private EventId _eventId;

    public LogBuilder(ILogger logger, LogLevel logLevel, string message, params object[] args)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _logEvent = new LogEvent(message, args);
        _level = logLevel;
        _eventId = 0;
        _exception = null;
    }

    public LogEvent LogEvent => _logEvent;

    public LogBuilder SetException(Exception exception)
    {
        _exception = exception;
        return this;
    }

    public LogBuilder SetLevel(LogLevel logLevel)
    {
        _level = logLevel;
        return this;
    }

    private LogBuilder SetMessage(string format, params object[] args)
    {
        _logEvent.SetFormattedMessage(format, args);
        return this;
    }

    public LogBuilder AddStatus(string status)
    {
        _logEvent.AddProp("Status", status);
        return this;
    }

    public LogBuilder AddUid(Guid? uid = null)
    {
        var value = uid.HasValue
            ? uid.Value.ToString()
            : "null";

        _logEvent.AddProp("uid", value);
        return this;
    }

    public LogBuilder AddErrors(ICollection<string> errors)
    {
        _logEvent.AddProp("Errors", string.Join(", ", errors));
        return this;
    }

    public LogBuilder AddHost(string host)
    {
        _logEvent.AddProp("Host", host);
        return this;
    }

    public LogBuilder AddFileName(string fileName)
    {
        _logEvent.AddProp("FileName", fileName);
        return this;
    }

    public LogBuilder AddSignatureName(string signatureName)
    {
        _logEvent.AddProp("SignatureName", signatureName);
        return this;
    }

    public LogBuilder AddHttpMethod(string method)
    {
        _logEvent.AddProp("HttpMethod", method);
        return this;
    }

    public LogBuilder AddProperty(string name, object value)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));

        _logEvent.AddProp(name, value);
        return this;
    }

    public LogBuilder SetEventId(EventId eventId)
    {
        _eventId = eventId;
        return this;
    }

    public void Write()
    {
        _logger.Log(_level, _eventId, _logEvent, _exception, LogEvent.Formatter);
    }
}
