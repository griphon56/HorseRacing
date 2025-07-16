using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Requests.CheckPlayerConnectedToGame
{
    public class CheckPlayerConnectedToGameRequestDto : BaseDto
    {
        /// <summary>
        /// Код пользователя
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Код игры
        /// </summary>
        public Guid GameId { get; set; }
    }
}
