using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class GameDeckCardDto : BaseDto
    {
        /// <summary>
        /// Масть
        /// </summary>
        public int CardSuit { get; set; }
        /// <summary>
        /// Номинал
        /// </summary>
        public int CardRank { get; set; }
        /// <summary>
        /// Позиция в колоде
        /// </summary>
        public int CardOrder { get; set; }
        /// <summary>
        /// Нахождение в игре
        /// </summary>
        public int Zone { get; set; }
    }
}
