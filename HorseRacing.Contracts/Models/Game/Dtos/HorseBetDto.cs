using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class HorseBetDto : BaseDto
    {
        /// <summary>
        /// Масть
        /// </summary>
        public int BetSuit { get; set; }
        /// <summary>
        /// Сумма ставки
        /// </summary>
        public int BetAmount { get; set; }
        /// <summary>
        /// Код пользователя
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// ФИО (логин)
        /// </summary>
        public string FullName { get; set; } = string.Empty;
    }
}
