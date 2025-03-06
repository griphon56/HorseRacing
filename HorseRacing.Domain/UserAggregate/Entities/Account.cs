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

        public static Account Create(AccountId id, int balance, UserId userId)
        {
            return new Account(id, balance, userId);
        }
    }
}
