using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate.ValueObjects;

namespace HorseRacing.Domain.GameAggregate.Entities
{
    /// <summary>
    /// Сущность "Результат игры"
    /// </summary>
    public class GameResult : EntityGuid<GameResultId>
    {
        /// <summary>
        /// Место, позиция
        /// </summary>
        public int Position { get; private set; }
        /// <summary>
        /// Масть
        /// </summary>
        public SuitType BetSuit { get; private set; }
        /// <summary>
        /// Код игры
        /// </summary>
        public GameId GameId { get; private set; }
        /// <summary>
        /// Код пользователя
        /// </summary>
        public UserId UserId { get; private set; }

        private GameResult () : base(GameResultId.CreateUnique()) { }

        private GameResult(GameResultId id, int position, SuitType betSuit, GameId gameId, UserId userId)
            : base(id ?? GameResultId.CreateUnique())
        {
            Position = position;
            BetSuit = betSuit;
            GameId = gameId;
            UserId = userId;
        }

        public static GameResult Create(GameResultId id, int position, SuitType betSuit, GameId gameId, UserId userId)
        {
            return new GameResult(id, position, betSuit, gameId, userId);
        }
    }
}
