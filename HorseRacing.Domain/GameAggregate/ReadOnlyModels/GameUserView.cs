using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.UserAggregate.ValueObjects;

namespace HorseRacing.Domain.GameAggregate.ReadOnlyModels
{
    public class GameUserView
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public required UserId UserId { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public required string FullName { get; set; }
        /// <summary>
        /// Сумма ставки
        /// </summary>
        public required decimal BetAmount { get; set; }
        /// <summary>
        /// Масть
        /// </summary>
        public required SuitType BetSuit { get; set; }
    }
}
