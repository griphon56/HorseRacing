using HorseRacing.Domain.Common.Models.Base;

namespace HorseRacing.Domain.EventLogAggregate.ValueObjects
{
    public class EventLogId : IdentityBaseInt
    {
        /// <summary>
        /// Конструктор без параметров <see cref="EventLogId"/> .
        /// </summary>
        private EventLogId() : base() { }
        /// <summary>
        /// Конструктор для создания на основе значения <see cref="int"/>
        /// </summary>
        public EventLogId(int value) : base(value) { }
        /// <summary>
        /// Создает уникальное значение.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Возвращает идентификатор <see cref="EventLogId"/></returns>
        public static EventLogId CreateUnique(int value)
        {
            return new(value);
        }
        /// <summary>
        /// Создает на основе значения <see cref="int"/>.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Возвращает идентификатор <see cref="EventLogId"/></returns>
        public static EventLogId Create(int value)
        {
            return new(value);
        }
    }
}
