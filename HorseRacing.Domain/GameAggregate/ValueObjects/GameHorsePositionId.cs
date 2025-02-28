using HorseRacing.Domain.Common.Models.Base;

namespace HorseRacing.Domain.GameAggregate.ValueObjects
{
    public class GameHorsePositionId : IdentityBaseGuid
    {
        /// <summary>
        /// Конструктор без параметров <see cref="GameHorsePositionId"/> .
        /// </summary>
        private GameHorsePositionId() : base()
        { }
        /// <summary>
        /// Конструктор для создания на основе значения <see cref="Guid"/>
        /// </summary>
        /// <param name="value">Значение.</param>
        public GameHorsePositionId(Guid value) : base(value) { }
        /// <summary>
        /// Создает уникальное значение.
        /// </summary>
        /// <returns>Возвращает идентификатор <see cref="GameHorsePositionId"/></returns>
        public static GameHorsePositionId CreateUnique()
        {
            return new(Guid.NewGuid());
        }
        /// <summary>
        /// Создает на основе значения <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Возвращает идентификатор <see cref="GameHorsePositionId"/></returns>
        public static GameHorsePositionId Create(Guid value)
        {
            return new(value);
        }
    }
}
