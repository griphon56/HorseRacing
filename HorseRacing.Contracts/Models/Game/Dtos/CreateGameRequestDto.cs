using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class CreateGameRequestDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
