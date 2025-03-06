using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class GetGameRequestDto : BaseDto
    {
        public Guid Id { get; set; }
    }
}
