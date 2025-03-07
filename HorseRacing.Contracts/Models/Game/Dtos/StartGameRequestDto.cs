using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class StartGameRequestDto : BaseDto
    {
        public Guid GameId { get; set; }
    }
}
