using HorseRacing.Common;
using HorseRacing.Common.Utilities;
using HorseRacing.Domain.EventLogAggregate.Enums;
using Microsoft.Extensions.Logging;

namespace HorseRacing.LoggerExtenstions.Common
{
    /// <summary>
	/// Информация о логируемом событии
	/// </summary>
	public class EventInfo
    {
        /// <summary>
        /// Наименование инициатора действия
        /// </summary>
        public string InitiatorInfoText = string.Empty;
        /// <summary>
        /// Уровень события
        /// </summary>
        public LogLevel Level;
        /// <summary>
        /// Подсистема
        /// </summary>
        public int Subsystem;
        /// <summary>
        /// Технологический процесс
        /// </summary>
        public int TechProcess;
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message = string.Empty;
        /// <summary>
        /// Тип события
        /// </summary>
        public int EventType;
        /// <summary>
        /// Заголовок события
        /// </summary>
        public string EventTitle = string.Empty;
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid? UserId;
        /// <summary>
        /// Аргументы
        /// </summary>
        public object?[] Args = [];

        public EventInfo() { }

        /// <summary>
        /// Конструктор для создания на основе параметров <see cref="EventInfo"/> .
        /// </summary>
        /// <param name="eventTitle">Заголовок события.</param>
        /// <param name="eventType">Тип события.</param>
        /// <param name="message">Описание события.</param>
        /// <param name="initiatorInfoText">Наименование инициатора действия.</param>
        /// <param name="level">Уровень события.</param>
        /// <param name="subsystem">Подсистема.</param>
        /// <param name="techProcess">Технологический процесс.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="args">Аргументы.</param>
        public EventInfo(string eventTitle, EventType eventType, string message, LogLevel level
            , SubsystemType subsystem, TechProcessType techProcess, Guid? userId = null
            , string initiatorInfoText = CommonSystemValues.DefaultSystemUserName, params object?[] args)
        {
            EventTitle = eventTitle;
            InitiatorInfoText = initiatorInfoText;
            EventType = (int)eventType;
            Message = message;
            Level = level;
            Subsystem = (int)subsystem;
            TechProcess = (int)techProcess;
            Args = args;
            UserId = userId;
        }
        /// <summary>
        /// Конструктор для создания на основе параметров <see cref="EventInfo"/> .
        /// </summary>
        /// <param name="eventType">Тип события.</param>
        /// <param name="message">Описание события.</param>
        /// <param name="initiatorInfoText">Наименование инициатора действия.</param>
        /// <param name="level">Уровень события.</param>
        /// <param name="subsystem">Подсистема.</param>
        /// <param name="techProcess">Технологический процесс.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="args">Аргументы.</param>
        public EventInfo(EventType eventType, string message, LogLevel level
            , SubsystemType subsystem, TechProcessType techProcess, Guid? userId = null
            , string initiatorInfoText = CommonSystemValues.DefaultSystemUserName, params object?[] args)
        {
            EventTitle = eventType.GetEnumDescription();
            InitiatorInfoText = initiatorInfoText;
            EventType = (int)eventType;
            Message = message;
            Level = level;
            Subsystem = (int)subsystem;
            TechProcess = (int)techProcess;
            Args = args;
            UserId = userId;
        }
    }
}