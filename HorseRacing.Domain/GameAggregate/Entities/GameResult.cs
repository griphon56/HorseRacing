using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace HorseRacing.Domain.GameAggregate.Entities
{
    /// <summary>
    /// Сущность "Результат игры"
    /// </summary>
    [Display(Description = "Результат игры")]
    public class GameResult : EntityGuid<GameResultId>
    {
        /// <summary>
        /// Место, которое заняла лошадь в игре
        /// </summary>
        public int Place {  get; private set; }
        /// <summary>
        /// Позиция лошади по окончанию игры 
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

        private GameResult(GameResultId id, int position, int place, SuitType betSuit, GameId gameId, UserId userId)
            : base(id ?? GameResultId.CreateUnique())
        {
            Place = place;
            Position = position;
            BetSuit = betSuit;
            GameId = gameId;
            UserId = userId;
        }

        public static GameResult Create(GameResultId id, int position, int place, SuitType betSuit, GameId gameId, UserId userId)
        {
            return new GameResult(id, position, place, betSuit, gameId, userId);
        }
    }
}
