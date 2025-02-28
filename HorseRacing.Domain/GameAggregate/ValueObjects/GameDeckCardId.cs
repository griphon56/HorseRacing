using HorseRacing.Domain.Common.Models.Base;

namespace HorseRacing.Domain.GameAggregate.ValueObjects
{
    public class GameDeckCardId : IdentityBaseGuid
    {
        /// <summary>
        /// Конструктор без параметров <see cref="GameDeckCardId"/> .
        /// </summary>
        private GameDeckCardId() : base()
        { }
        /// <summary>
        /// Конструктор для создания на основе значения <see cref="Guid"/>
        /// </summary>
        /// <param name="value">Значение.</param>
        public GameDeckCardId(Guid value) : base(value) { }
        /// <summary>
        /// Создает уникальное значение.
        /// </summary>
        /// <returns>Возвращает идентификатор <see cref="GameDeckCardId"/></returns>
        public static GameDeckCardId CreateUnique()
        {
            return new(Guid.NewGuid());
        }
        /// <summary>
        /// Создает на основе значения <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Возвращает идентификатор <see cref="GameDeckCardId"/></returns>
        public static GameDeckCardId Create(Guid value)
        {
            return new(value);
        }
    }
}
