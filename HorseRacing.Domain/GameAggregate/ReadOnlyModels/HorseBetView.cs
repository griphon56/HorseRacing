using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.UserAggregate.ValueObjects;

namespace HorseRacing.Domain.GameAggregate.ReadOnlyModels
{
    /// <summary>
    /// Модель содержит информацию о игроке и масти лошади
    /// </summary>
    public class HorseBetView
    {
        /// <summary>
        /// Масть
        /// </summary>
        public SuitType BetSuit { get; set; }
        /// <summary>
        /// Сумма ставки
        /// </summary>
        public decimal BetAmount { get; set; }
        /// <summary>
        /// Код пользователя
        /// </summary>
        public UserId UserId { get; set; }
        /// <summary>
        /// ФИО (логин)
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        public HorseBetView() { }

        public HorseBetView(SuitType betSuit, decimal betAmount, UserId userId, string fullName)
        {
            BetSuit = betSuit;
            BetAmount = betAmount;
            UserId = userId;
            FullName = fullName;
        }
    }
}
