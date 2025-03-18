using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate.ValueObjects;

namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class GameResultDto : BaseDto
    {
        /// <summary>
        /// Место, позиция
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// Масть
        /// </summary>
        public SuitType BetSuit { get; set; }
        /// <summary>
        /// Код игры
        /// </summary>
        public GameId GameId { get; set; }
        /// <summary>
        /// Код пользователя
        /// </summary>
        public UserId UserId { get; set; }
        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        public string FullName { get; set; } = string.Empty;
        /// <summary>
        /// Победитель
        /// </summary>
        public bool IsWinner { get; set; }
    }
}
