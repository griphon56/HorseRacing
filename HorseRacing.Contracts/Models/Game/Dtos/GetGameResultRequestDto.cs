using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class GetGameResultRequestDto : BaseDto
    {
        public Guid GameId { get; set; }
    }
}
