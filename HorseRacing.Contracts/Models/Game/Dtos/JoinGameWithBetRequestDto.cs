using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class JoinGameWithBetRequestDto : BaseDto
    {
        /// <summary>
        /// Код пользователя
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Код игры
        /// </summary>
        public Guid GameId { get; set; }
        /// <summary>
        /// Ставка
        /// </summary>
        public int BetAmount { get; set; }
        /// <summary>
        /// Масть
        /// </summary>
        public int BetSuit { get; set; }
    }
}
