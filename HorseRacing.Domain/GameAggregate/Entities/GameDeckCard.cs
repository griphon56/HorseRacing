using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace HorseRacing.Domain.GameAggregate.Entities
{
    /// <summary>
    /// Сущность "Игровая колода"
    /// </summary>
    [Display(Description = "Игровая колода")]
    public class GameDeckCard : EntityGuid<GameDeckCardId>
    {
        /// <summary>
        /// Масть
        /// </summary>
        public SuitType CardSuit { get; private set; }
        /// <summary>
        /// Номинал
        /// </summary>
        public RankType CardRank { get; private set; }
        /// <summary>
        /// Позиция в колоде
        /// </summary>
        public int CardOrder { get; private set; }
        /// <summary>
        /// Нахождение в игре
        /// </summary>
        public ZoneType Zone { get; private set; }
        /// <summary>
        /// Код игры
        /// </summary>
        public GameId GameId { get; private set; }

        private GameDeckCard () : base(GameDeckCardId.CreateUnique()) { }

        private GameDeckCard(GameDeckCardId id, GameId gameId, SuitType cardSuit, RankType cardRank, int cardOrder, ZoneType zone)
            : base(id ?? GameDeckCardId.CreateUnique())
        {
            GameId = gameId;
            CardSuit = cardSuit;
            CardRank = cardRank;
            CardOrder = cardOrder;
            Zone = zone;
        }

        public static GameDeckCard Create(GameDeckCardId id, GameId gameId, SuitType cardSuit, RankType cardRank, int cardOrder, ZoneType zone)
        {
            return new GameDeckCard(id, gameId, cardSuit, cardRank, cardOrder, zone);
        }
    }
}
