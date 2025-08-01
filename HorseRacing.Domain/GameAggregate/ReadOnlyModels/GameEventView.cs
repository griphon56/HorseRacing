using HorseRacing.Domain.GameAggregate.Enums;

namespace HorseRacing.Domain.GameAggregate.ReadOnlyModels
{
    public class GameEventView
    {
        /// <summary>
        /// Тип события
        /// </summary>
        public GameEventType EventType { get; set; }
        /// <summary>
        /// Масть карты
        /// </summary>
        public SuitType CardSuit { get; set; }
        /// <summary>
        /// Номинал карты
        /// </summary>
        public RankType CardRank { get; set; }
        /// <summary>
        /// Позиция карты в колоде
        /// </summary>
        public int CardOrder { get; set; }
        /// <summary>
        /// Масть лошади
        /// </summary>
        public SuitType HorseSuit { get; set; }
        /// <summary>
        /// Позиция лошади
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// Дата события
        /// </summary>
        public DateTime EventDate { get; set; }

        public GameEventView(GameEventType eventType, SuitType cardSuit, RankType cardRank, int cardOrder, SuitType horseSuit, int position, DateTime eventDate)
        {
            EventType = eventType;
            CardSuit = cardSuit;
            CardRank = cardRank;
            CardOrder = cardOrder;
            HorseSuit = horseSuit;
            Position = position;
            EventDate = eventDate;
        }
    }
}
