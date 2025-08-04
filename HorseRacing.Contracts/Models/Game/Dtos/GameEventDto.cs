using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class GameEventDto : BaseDto
    {
        /// <summary>
        /// Порядковый номер шага (1,2,3,…)
        /// </summary>
        public int Step { get; private set; }
        /// <summary>
        /// Тип события
        /// </summary>
        public int EventType { get; private set; }
        /// <summary>
        /// Масть карты
        /// </summary>
        public int? CardSuit { get; private set; }
        /// <summary>
        /// Номинал карты
        /// </summary>
        public int? CardRank { get; private set; }
        /// <summary>
        /// Позиция карты в колоде
        /// </summary>
        public int? CardOrder { get; private set; }
        /// <summary>
        /// Масть лошади
        /// </summary>
        public int? HorseSuit { get; private set; }
        /// <summary>
        /// Позиция лошади
        /// </summary>
        public int? Position { get; private set; }
        /// <summary>
        /// Дата события
        /// </summary>
        public DateTime EventDate { get; private set; }
    }
}
