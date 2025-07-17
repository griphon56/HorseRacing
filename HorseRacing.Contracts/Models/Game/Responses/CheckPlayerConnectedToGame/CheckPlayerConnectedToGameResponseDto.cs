using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Responses.CheckPlayerConnectedToGame
{
    public class CheckPlayerConnectedToGameResponseDto : BaseDto
    {
        /// <summary>
        /// Признак подключения к игре
        /// </summary>
        public bool IsConnected { get; set; }
    }
}
