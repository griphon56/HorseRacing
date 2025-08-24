using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Responses.GetLobbyUsersWithBets
{
    public class GetLobbyUsersWithBetsResponseDto : BaseDto
    {
        /// <summary>
        /// Код игры
        /// </summary>
        public Guid GameId { get; set; }
        /// <summary>
        /// Наименование игры
        /// </summary>
        public string GameName { get; set; } = string.Empty;
        /// <summary>
        /// Общий банк ставок
        /// </summary>
        public decimal TotalBank { get; set; }
        /// <summary>
        /// Список пользователей в лобби с их ставками
        /// </summary>
        public List<GameUserDto> Players { get; set; } = new();
    }
}
