using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Domain.GameAggregate.Enums;

namespace HorseRacing.Contracts.Models.Game.Responses.GetGameResult
{
    public class GetGameResultResponseDto : BaseDto
    {
        /// <summary>
        /// Место, которое заняла лошадь в игре
        /// </summary>
        public int Place { get; set; }
        /// <summary>
        /// Масть
        /// </summary>
        public SuitType BetSuit { get; set; }
        /// <summary>
        /// Код игры
        /// </summary>
        public Guid GameId { get; set; }
        /// <summary>
        /// Код пользователя
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        public string FullName { get; set; } = string.Empty;
    }
}
