using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.EventLogAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate.ValueObjects;

namespace HorseRacing.Domain.EventLogAggregate
{
    /// <summary>
	/// Событие
	/// </summary>
	public class EventLog : AggregateRootInt<EventLogId>
    {
        /// <summary>
        /// Конструктор без параметров <see cref="EventLog"/> .
        /// </summary>
        public EventLog() : base(EventLogId.CreateUnique(0)) { }
        /// <summary>
        /// Тип события
        /// </summary>
        public int? EventType { get; private set; } = null;
        /// <summary>
        /// Заголовок события
        /// </summary>
        public string? EventTitle { get; private set; } = "";
        /// <summary>
        /// Описание события
        /// </summary>
        public string? Message { get; private set; } = "";
        /// <summary>
        /// Шаблон описания события
        /// </summary>
        public string? MessageTemplate { get; private set; } = String.Empty;
        /// <summary>
        /// Наименование инициатора действия
        /// </summary>
        public string? InitiatorInfoText { get; private set; } = "";
        /// <summary>
        /// Дата события
        /// </summary>
        public DateTime? TimeStamp { get; private set; } = null;
        /// <summary>
        /// Уровень события
        /// </summary>
        public string? Level { get; private set; } = "";
        /// <summary>
        /// Подсистема
        /// </summary>
        public int? Subsystem { get; private set; } = null;
        /// <summary>
        /// Технологический процесс
        /// </summary>
        public int? TechProcess { get; private set; } = null;
        /// <summary>
        /// Информация о возникшем исключении
        /// </summary>
        public string? Exception { get; private set; } = "";
        /// <summary>
        /// Сведения о свойствах
        /// </summary>
        public string Properties { get; private set; } = String.Empty;
        /// <summary>
        /// Код пользователя
        /// </summary>
        public UserId? UserId { get; private set; } = null;

        /// <summary>
        /// Конструктор для создания на основе параметров <see cref="EventLog"/> .
        /// </summary>
        /// <param name="eventLogId">Идентификатор события</param>
        /// <param name="eventType">Тип события.</param>
        /// <param name="eventTitle">Заголовок события.</param>
        /// <param name="message">Описание события.</param>
        /// <param name="messageTemplate">Шаблон описания события.</param>
        /// <param name="initiatorInfoText">Наименование инициатора действия.</param>
        /// <param name="timeStamp">Дата события.</param>
        /// <param name="level">Уровень события.</param>
        /// <param name="exception">Информация о возникшем исключении.</param>
        /// <param name="properties">Сведения о свойствах.</param>
        /// <param name="userId">Код пользователя.</param>
        private EventLog(EventLogId eventLogId, string eventTitle, string initiatorInfoText, DateTime timeStamp, string properties, int? eventType = null,
            string? message = null, string? messageTemplate = null, string? level = null, string? exception = null, UserId? userId = null, int? subsystem = null, int? techProcess = null)
                : base(eventLogId ?? EventLogId.CreateUnique(0))
        {
            EventType = eventType;
            EventTitle = eventTitle;
            Message = message;
            MessageTemplate = messageTemplate;
            InitiatorInfoText = initiatorInfoText;
            TimeStamp = timeStamp;
            Level = level;
            Exception = exception;
            Properties = properties;
            UserId = userId;
            Subsystem = subsystem;
            TechProcess = techProcess;
        }
        /// <summary>
        /// Создает <see cref="EventLog"/>.
        /// </summary>
        /// <param name="eventLogId">Идентификатор события</param>
        /// <param name="eventType">Тип события.</param>
        /// <param name="eventTitle">Заголовок события.</param>
        /// <param name="message">Описание события.</param>
        /// <param name="messageTemplate">Шаблон описания события.</param>
        /// <param name="initiatorInfoText">Наименование инициатора действия.</param>
        /// <param name="timeStamp">Дата события.</param>
        /// <param name="level">Уровень события.</param>
        /// <param name="exception">Информация о возникшем исключении.</param>
        /// <param name="properties">Сведения о свойствах.</param>
        /// <param name="userId">Код пользователя.</param>
        /// <returns>Возвращает значение <see cref="EventLog"/></returns>
        public static EventLog Create(EventLogId eventLogId, string eventTitle, string initiatorInfoText
            , DateTime timeStamp, string properties, int? eventType = null, string? message = null
            , string? messageTemplate = null, string? level = null, string? exception = null, UserId? userId = null
            , int? subsystem = null, int? techProcess = null)
        {
            var EventLog = new EventLog(eventLogId, eventTitle, initiatorInfoText, timeStamp, properties, eventType, message, messageTemplate, level, exception, userId, subsystem, techProcess);

            return EventLog;
        }
    }
}
