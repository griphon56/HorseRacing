using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace HorseRacing.Domain.UserAggregate.Entities
{
    /// <summary>
    /// Сущность "Баланса пользователя"
    /// </summary>
    [Display(Description = "Баланс пользователя")]
    public partial class Account : EntityGuid<AccountId>
    {
        /// <summary>
        /// Баланс счета
        /// </summary>
        public int Balance { get; private set; }
        /// <summary>
        /// Код пользователя
        /// </summary>
        public UserId UserId { get; private set; }

        private Account() : base(AccountId.CreateUnique()) { }

        private Account(AccountId id, int balance, UserId userId) : base(id ?? AccountId.CreateUnique())
        {
            Balance = balance;
            UserId = userId;
        }

        /// <summary>
        /// Метод создания сущности "Баланс пользователя"
        /// </summary>
        /// <param name="id">Код аккаунта</param>
        /// <param name="balance">Баланс</param>
        /// <param name="userId">Код пользователя</param>
        public static Account Create(AccountId id, int balance, UserId userId)
        {
            return new Account(id, balance, userId);
        }
        /// <summary>
        /// Метод списания баланса
        /// </summary>
        /// <param name="balance">Баланс</param>
        public void DebitBalance(int coins)
        {
            Balance -= coins;
        }
        /// <summary>
        /// Метод зачисления баланса
        /// </summary>
        /// <param name="balance">Баланс</param>
        public void CreditBalance(int coins)
        {
            Balance += coins;
        }
    }
}
