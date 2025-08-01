using HorseRacing.Domain.GameAggregate.Enums;

namespace HorseRacing.Domain.GameAggregate.ReadOnlyModels
{
    public class GameDeckCardView
    {
        /// <summary>
        /// Масть
        /// </summary>
        public SuitType CardSuit { get; set; }
        /// <summary>
        /// Номинал
        /// </summary>
        public RankType CardRank { get; set; }
        /// <summary>
        /// Позиция в колоде
        /// </summary>
        public int CardOrder { get; set; }
        /// <summary>
        /// Нахождение в игре
        /// </summary>
        public ZoneType Zone { get; set; }

        public GameDeckCardView(SuitType cardSuit, RankType cardRank, int cardOrder, ZoneType zone)
        {
            CardSuit = cardSuit;
            CardRank = cardRank;
            CardOrder = cardOrder;
            Zone = zone;
        }
    }
}
