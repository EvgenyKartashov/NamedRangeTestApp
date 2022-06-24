using Microsoft.Extensions.Logging;
using System.Collections;
using System.Text.Json;

namespace Core.Logging.Fluent;

public class LogEvent : ILogger, IReadOnlyList<KeyValuePair<string, object>>
{
    private string Format { get; set; }
    private object[] Parameters { get; set; }

    private IReadOnlyList<KeyValuePair<string, object>> _logValues;
    private List<KeyValuePair<string, object>> _extraProperties;

    public LogEvent(string format, params object[] values)
    {
        Format = format;
        Parameters = values;
    }

    public LogEvent AddProp(string name, object value)
    {
        var properties = _extraProperties ??= new List<KeyValuePair<string, object>>();
        var needSerialize = name.StartsWith("@");
        if (needSerialize)
        {
            var key = name.Substring(1);
            var data = JsonSerializer.Serialize(value);//JsonConvert.SerializeObject(value);
            properties.Add(new KeyValuePair<string, object>(key, data));
            return this;
        }

        properties.Add(new KeyValuePair<string, object>(name, value));
        return this;
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        if (MessagePropertyCount == 0)
        {
            return ExtraPropertyCount > 0 ? _extraProperties.GetEnumerator() : System.Linq.Enumerable.Empty<KeyValuePair<string, object>>().GetEnumerator();
        }

        return ExtraPropertyCount > 0 ? System.Linq.Enumerable.Concat(_extraProperties, LogValues).GetEnumerator() : LogValues.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public KeyValuePair<string, object> this[int index]
    {
        get
        {
            var extraCount = ExtraPropertyCount;
            return index < extraCount ? _extraProperties[index] : LogValues[index - extraCount];
        }
    }

    public int Count => MessagePropertyCount + ExtraPropertyCount;

    public override string ToString() => LogValues.ToString();

    private IReadOnlyList<KeyValuePair<string, object>> LogValues
    {
        get
        {
            if (_logValues == null)
                this.LogDebug(Format, Parameters);
            return _logValues;
        }
    }

    private int ExtraPropertyCount => _extraProperties?.Count ?? 0;

    private int MessagePropertyCount
    {
        get
        {
            if (LogValues.Count > 1 && !string.IsNullOrEmpty(LogValues[0].Key) && !char.IsDigit(LogValues[0].Key[0]))
                return LogValues.Count;
            else
                return 0;
        }
    }

    void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        _logValues = state as IReadOnlyList<KeyValuePair<string, object>> ?? Array.Empty<KeyValuePair<string, object>>();
    }

    bool ILogger.IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    IDisposable ILogger.BeginScope<TState>(TState state)
    {
        throw new NotSupportedException();
    }

    public static Func<LogEvent, Exception, string> Formatter { get; } = (l, e) =>
    {
        //var t = l.LogValues;
        return l.LogValues.ToString();
    };


    //---------------------Fluent----------------------------

    public void SetFormattedMessage(string format, params object[] values)
    {
        this.Parameters = values;
        this.Format = format;
    }
}
