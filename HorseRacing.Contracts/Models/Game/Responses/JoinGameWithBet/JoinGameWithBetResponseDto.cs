using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Responses.JoinGameWithBet
{
    public class JoinGameWithBetResponseDto : BaseDto
    {
        /// <summary>
        /// Признак подключения последнего игрока
        /// </summary>
        public bool IsLastPlayer { get; set; }
    }
}
