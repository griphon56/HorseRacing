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
        /// Порядковый номер шага (1,2,3,…)
        /// </summary>
        public int Step { get; private set; }
        /// <summary>
        /// Тип события
        /// </summary>
        public GameEventType EventType { get; private set; }
        /// <summary>
        /// Масть карты
        /// </summary>
        public SuitType? CardSuit { get; private set; }
        /// <summary>
        /// Номинал карты
        /// </summary>
        public RankType? CardRank { get; private set; }
        /// <summary>
        /// Позиция карты в колоде
        /// </summary>
        public int? CardOrder { get; private set; }
        /// <summary>
        /// Масть лошади
        /// </summary>
        public SuitType? HorseSuit { get; private set; }
        /// <summary>
        /// Позиция лошади
        /// </summary>
        public int? Position { get; private set; }
        /// <summary>
        /// Дата события
        /// </summary>
        public DateTime EventDate { get; private set; }
        /// <summary>
        /// Код игры
        /// </summary>
        public GameId GameId { get; private set; }

        private GameEvent() : base(GameEventId.CreateUnique()) { }

        private GameEvent(GameEventId id, GameId gameId, int step, GameEventType eventType, DateTime eventDate
            , SuitType? cardSuit, RankType? cardRank, int? cardOrder, SuitType? horseSuit, int? position)
            : base(id ?? GameEventId.CreateUnique())
        {
            Step = step;
            EventType = eventType;
            CardSuit = cardSuit;
            CardRank = cardRank;
            CardOrder = cardOrder;
            HorseSuit = horseSuit;
            Position = position;
            EventDate = eventDate;
            GameId = gameId;
        }

        public static GameEvent Create(GameId gameId, int step, GameEventType eventType, SuitType? cardSuit = null
            , RankType? cardRank = null, int? cardOrder = null, SuitType? horseSuit = null, int? position = null)
        {
            return new GameEvent(GameEventId.CreateUnique(), gameId, step, eventType, DateTime.UtcNow, cardSuit, cardRank, cardOrder, horseSuit, position);
        }
    }
}
