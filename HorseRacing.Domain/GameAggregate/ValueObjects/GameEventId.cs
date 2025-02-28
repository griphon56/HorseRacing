using HorseRacing.Domain.Common.Models.Base;

namespace HorseRacing.Domain.GameAggregate.ValueObjects
{
    public class GameEventId : IdentityBaseGuid
    {
        /// <summary>
        /// Конструктор без параметров <see cref="GameEventId"/> .
        /// </summary>
        private GameEventId() : base()
        { }
        /// <summary>
        /// Конструктор для создания на основе значения <see cref="Guid"/>
        /// </summary>
        /// <param name="value">Значение.</param>
        public GameEventId(Guid value) : base(value) { }
        /// <summary>
        /// Создает уникальное значение.
        /// </summary>
        /// <returns>Возвращает идентификатор <see cref="GameEventId"/></returns>
        public static GameEventId CreateUnique()
        {
            return new(Guid.NewGuid());
        }
        /// <summary>
        /// Создает на основе значения <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Возвращает идентификатор <see cref="GameEventId"/></returns>
        public static GameEventId Create(Guid value)
        {
            return new(value);
        }
    }
}
