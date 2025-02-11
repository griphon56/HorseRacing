using HorseRacing.LoggerExtenstions.Common;
using Microsoft.Extensions.Logging;

namespace HorseRacing.LoggerExtenstions.Services
{
    /// <summary>
	/// Пользовательские расширения журнала.
	/// </summary>
	public static class CustomLoggerExtensions
    {
        public const string InitiatorInfoTextCustomColumnName = "InitiatorInfoText";
        public const string EventTypeCustomColumnName = "EventType";
        public const string EventTitleCustomColumnName = "EventTitle";
        public const string UserIdCustomColumnName = "UserId";
        public const string SubsystemCustomColumnName = "Subsystem";
        public const string TechProcessCustomColumnName = "TechProcess";
        /// <summary>
        /// Запись в журнал событий информации о событие с указанием наименования инициатора действия
        /// </summary>
        /// <param name="logger">Регистратор.</param>
        /// <param name="eventInfo">Информация о логируемом событии.</param>
        public static void LogWithInitiatorInfoText(this ILogger logger, EventInfo eventInfo)
        {
            using (logger.BeginScope(new Dictionary<string, object> {
                { InitiatorInfoTextCustomColumnName, eventInfo.InitiatorInfoText },
                { EventTypeCustomColumnName, eventInfo.EventType },
                { EventTitleCustomColumnName, eventInfo.EventTitle },
                { UserIdCustomColumnName, eventInfo.UserId },
                { SubsystemCustomColumnName, eventInfo.Subsystem },
                { TechProcessCustomColumnName, eventInfo.TechProcess }
            }))
            {
                BaseLog(logger, eventInfo.Level, eventInfo.Message, eventInfo.Args);
            }
        }
        /// <summary>
        /// Основывает журнал.
        /// </summary>
        /// <param name="logger">Регистратор.</param>
        /// <param name="level">Уровень.</param>
        /// <param name="message">Сообщение.</param>
        /// <param name="args">Аргументы.</param>
        private static void BaseLog(ILogger logger, LogLevel level, string message, params object?[] args)
        {
            logger.Log(level, message, args);
        }
    }
}
