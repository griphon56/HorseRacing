using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Domain.GameAggregate.Enums;

namespace HorseRacing.Contracts.Models.Game.Responses.GetGame
{
    public class GetGameResponseDto : BaseDto
    {
        /// <summary>
        /// Код игры
        /// </summary>
        public Guid GameId { get; set; }
        /// <summary>
        /// Статус игры <see cref="StatusType"/>
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Режим игры <see cref="GameModeType"/>
        /// </summary> 
        public int Mode { get; set; }
        /// <summary>
        /// Предопределенная ставка при создании игры
        /// </summary>
        public decimal? DefaultBet { get; set; }
        /// <summary>
        /// Наименование комнаты
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Дата начала игры
        /// </summary>
        public DateTime DateStart { get; set; }
        /// <summary>
        /// Дата окончания
        /// </summary>
        public DateTime? DateEnd { get; set; }

    }
}
