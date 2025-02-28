using HorseRacing.Domain.Common.Models.Base;

namespace HorseRacing.Domain.UserAggregate.ValueObjects
{
    public class AccountId : IdentityBaseGuid
    {
        /// <summary>
        /// Конструктор без параметров <see cref="AccountId"/>.
        /// </summary>
        private AccountId() : base()
        { }
        /// <summary>
        /// Конструктор для создания на основе значения <see cref="Guid"/>
        /// </summary>
        /// <param name="value">Значение.</param>
        public AccountId(Guid value) : base(value) { }
        /// <summary>
        /// Создает уникальное значение.
        /// </summary>
        /// <returns>Возвращает идентификатор <see cref="AccountId"/></returns>
        public static AccountId CreateUnique()
        {
            return new(Guid.NewGuid());
        }
        /// <summary>
        /// Создает на основе значения <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Возвращает идентификатор <see cref="AccountId"/></returns>
        public static AccountId Create(Guid value)
        {
            return new(value);
        }
    }
}
