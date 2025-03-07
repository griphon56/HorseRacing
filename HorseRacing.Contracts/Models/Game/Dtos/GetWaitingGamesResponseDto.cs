using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class GetWaitingGamesResponseDto : BaseDto
    {
        public List<GameDto> Games { get; set; } = new List<GameDto>();
    }
}
