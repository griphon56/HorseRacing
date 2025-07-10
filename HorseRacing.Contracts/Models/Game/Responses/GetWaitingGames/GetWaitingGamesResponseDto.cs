using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Responses.GetWaitingGames
{
    public class GetWaitingGamesResponseDto : BaseDto
    {
        public List<GameDto> Games { get; set; } = new List<GameDto>();
    }
}
