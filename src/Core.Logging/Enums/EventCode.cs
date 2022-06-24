using System.ComponentModel;

namespace Core.Logging.Enums;

public enum EventCode
{
    Unspecified = 0,

    [Description("Критическая ошибка")] FatalUnspecified = LogConstants.EventCodeWarningLevelMultiplier * 1,
    [Description("Критическая ошибка в методе действия контроллера")] FatalControllerActionException = FatalUnspecified + 1,
    [Description("Критическая ошибка при работе с транзакциями БД")] FatalDbTransaction = FatalUnspecified + 2,
    [Description("Критическая ошибка при выполнении команды в БД")] FatalDbCommand = FatalUnspecified + 3,
    [Description("Критическая ошибка авторизации")] FatalAuthorization = FatalUnspecified + 6,
    [Description("Критическая ошибка Javascript")] FatalJavascript = FatalUnspecified + 40,

    [Description("Ошибка")] ErrorUnspecified = LogConstants.EventCodeWarningLevelMultiplier * 2,
    [Description("Ошибка уровня приложения")] ErrorApplication = ErrorUnspecified + 1,
    [Description("Ошибка при подключении к БД")] ErrorDbConnection = ErrorUnspecified + 10,
    [Description("Ошибка в SQL-запросе")] ErrorInSql = ErrorUnspecified + 12,
    [Description("Ошибка в методе действия контроллера")] ErrorControllerActionException = ErrorUnspecified + 13,
    [Description("Ошибка авторизации")] ErrorAuthorization = ErrorUnspecified + 14,
    [Description("Ошибка в процессе десериализации")] ErrorDeserialization = ErrorUnspecified + 15,
    [Description("Ошибка уведомления")] ErrorNotification = ErrorUnspecified + 16,
    [Description("Ошибка SMTP")] ErrorSmtp = ErrorUnspecified + 17,
    [Description("Ошибка отправления письма")] ErrorSendMail = ErrorUnspecified + 18,
    [Description("Ошибка Api-контроллера")] ErrorWebApi = ErrorUnspecified + 30,
    [Description("Ошибка Javascript")] ErrorJavascript = ErrorUnspecified + 40,
    [Description("Необработанная ошибка Javascript")] ErrorJavascriptUnhandled = ErrorUnspecified + 41,
    [Description("Ошибка в OData")] ErrorOData = ErrorUnspecified + 54,

    [Description("Предупреждение")] WarningUnspecified = LogConstants.EventCodeWarningLevelMultiplier * 3,
    [Description("Предупреждение о приложении")] WarningApplication = WarningUnspecified + 1,
    [Description("Доступ запрещен")] AccessDenied = WarningUnspecified + 2,
    [Description("Предупреждение Api-контроллера")] WarningWebApi = WarningUnspecified + 30,
    [Description("Предупреждение Javascript")] WarningJavascript = WarningUnspecified + 40,

    [Description("Информация")] InfoUnspecified = LogConstants.EventCodeWarningLevelMultiplier * 4,
    [Description("Информация о приложении")] InfoApplication = InfoUnspecified + 1,
    [Description("Информация Api-контроллера")] InfoWebApi = InfoUnspecified + 30,
    [Description("Информация Javascript")] InfoJavascript = InfoUnspecified + 40,

    [Description("Отладочная информация")] DebugUnspecified = LogConstants.EventCodeWarningLevelMultiplier * 5,
    [Description("Отладочная информация о работе с транзакциями БД")] DebugDbTransaction = DebugUnspecified + 5,
    [Description("Отладочная информация Api-контроллера")] DebugWebApi = DebugUnspecified + 30,
    [Description("Отладочная информация Javascript")] DebugJavascript = DebugUnspecified + 40,

    [Description("Трассировка")] TraceUnspecified = LogConstants.EventCodeWarningLevelMultiplier * 6,
    [Description("Трассировка методов действия в контроллере")] TraceControllerActionExecuting = TraceUnspecified + 1,
    [Description("Трассировка подключения к БД")] TraceDbConnection = TraceUnspecified + 2,
    [Description("Трассировка трансакций БД")] TraceDbTransaction = TraceUnspecified + 3,
    [Description("Трассировка команд для БД")] TraceDbCommand = TraceUnspecified + 4,
    [Description("Трассировка авторизации")] TraceAuthorization = TraceUnspecified + 5,
    [Description("Трассировка Api-контроллера")] TraceWebApi = TraceUnspecified + 30,
    [Description("Трассировка Javascript")] TraceJavascript = TraceUnspecified + 40,
}
