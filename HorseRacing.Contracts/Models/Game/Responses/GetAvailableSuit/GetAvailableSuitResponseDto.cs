using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Domain.GameAggregate.Enums;

namespace HorseRacing.Contracts.Models.Game.Responses.GetAvailableSuit
{
    public class GetAvailableSuitResponseDto : BaseDto
    {
        /// <summary>
        /// Код масти
        /// </summary>
        public SuitType Suit { get; set; }
        /// <summary>
        /// Название масти
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
