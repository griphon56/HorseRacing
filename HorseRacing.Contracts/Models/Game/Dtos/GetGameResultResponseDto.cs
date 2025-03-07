using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class GetGameResultResponseDto : BaseDto
    {
        public List<GameResultDto> GameResults { get; set; } = new List<GameResultDto>();
    }
}
