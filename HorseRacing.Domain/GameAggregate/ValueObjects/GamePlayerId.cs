using HorseRacing.Domain.Common.Models.Base;

namespace HorseRacing.Domain.GameAggregate.ValueObjects
{
    public class GamePlayerId : IdentityBaseGuid
    {
        /// <summary>
        /// Конструктор без параметров <see cref="GamePlayerId"/> .
        /// </summary>
        private GamePlayerId() : base() { }
        /// <summary>
        /// Конструктор для создания на основе значения <see cref="Guid"/>
        /// </summary>
        /// <param name="value">Значение.</param>
        public GamePlayerId(Guid value) : base(value) { }
        /// <summary>
        /// Создает уникальное значение.
        /// </summary>
        /// <returns>Возвращает идентификатор <see cref="GamePlayerId"/></returns>
        public static GamePlayerId CreateUnique()
        {
            return new(Guid.NewGuid());
        }
        /// <summary>
        /// Создает на основе значения <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Возвращает идентификатор <see cref="GamePlayerId"/></returns>
        public static GamePlayerId Create(Guid value)
        {
            return new(value);
        }
    }
}
