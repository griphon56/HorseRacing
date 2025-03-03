using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace HorseRacing.Domain.GameAggregate.Entities
{
    /// <summary>
    /// Сущность "События игры"
    /// </summary>
    [Display(Description = "События игры")]
    public class GameEvent : EntityGuid<GameEventId>
    {
        /// <summary>
        /// Тип события
        /// </summary>
        public GameEventType EventType { get; private set; }
        /// <summary>
        /// Масть карты
        /// </summary>
        public SuitType CardSuit { get; private set; }
        /// <summary>
        /// Номинал карты
        /// </summary>
        public RankType CardRank { get; private set; }
        /// <summary>
        /// Позиция карты в колоде
        /// </summary>
        public int CardOrder { get; private set; }
        /// <summary>
        /// Масть лошади
        /// </summary>
        public SuitType HorseSuit { get; private set; }
        /// <summary>
        /// Позиция лошади
        /// </summary>
        public int Position { get; private set; }
        /// <summary>
        /// Дата события
        /// </summary>
        public DateTime EventDate { get; private set; }
        /// <summary>
        /// Код игры
        /// </summary>
        public GameId GameId { get; private set; }

        private GameEvent() : base(GameEventId.CreateUnique()) { }

        public GameEvent(GameEventId id, GameId gameId, GameEventType eventType, SuitType cardSuit, RankType cardRank
            , int cardOrder, SuitType horseSuit, int position, DateTime eventDate)
            : base(id ?? GameEventId.CreateUnique())
        {
            GameId = gameId;
            EventType = eventType;
            CardSuit = cardSuit;
            CardRank = cardRank;
            CardOrder = cardOrder;
            HorseSuit = horseSuit;
            Position = position;
            EventDate = eventDate;
        }

        public static GameEvent Create(GameEventId id, GameId gameId, GameEventType eventType, SuitType cardSuit, RankType cardRank
            , int cardOrder, SuitType horseSuit, int position, DateTime eventDate)
        {
            return new GameEvent(id, gameId, eventType, cardSuit, cardRank, cardOrder, horseSuit, position, eventDate);
        }
    }
}
