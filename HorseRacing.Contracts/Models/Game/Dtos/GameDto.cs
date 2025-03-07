using HorseRacing.Domain.GameAggregate.Enums;

namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class GameDto
    {
        /// <summary>
        /// Код игры
        /// </summary>
        public Guid GameId { get; set; }
        /// <summary>
        /// Статус игры
        /// </summary>
        public StatusType Status { get; set; }
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
