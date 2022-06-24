namespace Core.Logging.Enums;

/// <summary>
/// Список ключей параметров записи в лог.
/// НЕЛЬЗЯ ДОБАВЛЯТЬ СЮДА КЛЮЧ С ИМЕНАМИ "EventCode", "Login", "ASPNETSessionId"!
/// </summary>
public enum EventParameterKey
{
    SqlQuery,
    SqlParams,
    SqlQueryDuration,
    ActionName,
    ControllerName,
    MethodName,
    EfEventName,
    EfDbContexts,
    BrowserName,
    BrowserVersion,
    BrowserPlatform,
    UserAgent,
    UserHostAddress,
    FileName,
}
