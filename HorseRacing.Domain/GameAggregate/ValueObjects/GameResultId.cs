using HorseRacing.Domain.Common.Models.Base;

namespace HorseRacing.Domain.GameAggregate.ValueObjects
{
    public class GameResultId : IdentityBaseGuid
    {
        /// <summary>
        /// Конструктор без параметров <see cref="GameResultId"/> .
        /// </summary>
        private GameResultId() : base()
        { }
        /// <summary>
        /// Конструктор для создания на основе значения <see cref="Guid"/>
        /// </summary>
        /// <param name="value">Значение.</param>
        public GameResultId(Guid value) : base(value) { }
        /// <summary>
        /// Создает уникальное значение.
        /// </summary>
        /// <returns>Возвращает идентификатор <see cref="GameResultId"/></returns>
        public static GameResultId CreateUnique()
        {
            return new(Guid.NewGuid());
        }
        /// <summary>
        /// Создает на основе значения <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Возвращает идентификатор <see cref="GameResultId"/></returns>
        public static GameResultId Create(Guid value)
        {
            return new(value);
        }
    }
}
