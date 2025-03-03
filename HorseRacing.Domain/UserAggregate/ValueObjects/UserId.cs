using HorseRacing.Domain.Common.Models.Base;

namespace HorseRacing.Domain.UserAggregate.ValueObjects
{
    public class UserId : IdentityBaseGuid
    {
        /// <summary>
        /// Конструктор без параметров <see cref="UserId"/> .
        /// </summary>
        private UserId() : base() { }
        /// <summary>
        /// Конструктор для создания на основе значения <see cref="Guid"/>
        /// </summary>
        /// <param name="value">Значение.</param>
        public UserId(Guid value) : base(value) { }
        /// <summary>
        /// Создает уникальное значение.
        /// </summary>
        /// <returns>Возвращает идентификатор <see cref="UserId"/></returns>
        public static UserId CreateUnique()
        {
            return new(Guid.NewGuid());
        }
        /// <summary>
        /// Создает на основе значения <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Возвращает идентификатор <see cref="UserId"/></returns>
        public static UserId Create(Guid value)
        {
            return new(value);
        }
    }
}
