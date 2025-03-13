using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace HorseRacing.Domain.GameAggregate.Entities
{
    /// <summary>
    /// Сущность "Игрок"
    /// </summary>
    [Display(Description = "Игрок")]
    public class GamePlayer : EntityGuid<GamePlayerId>
    {
        /// <summary>
        /// Ставка
        /// </summary>
        public int BetAmount { get; private set; }
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

        private GamePlayer() : base(GamePlayerId.CreateUnique()) { }

        private GamePlayer(GamePlayerId id, int betAmount, SuitType betSuit, GameId gameId, UserId userId)
             : base(id ?? GamePlayerId.CreateUnique())
        {
            BetAmount = betAmount;
            BetSuit = betSuit;
            GameId = gameId;
            UserId = userId;
        }

        public static GamePlayer Create(GamePlayerId id, int betAmount, SuitType betSuit, GameId gameId, UserId userId)
        {
            return new GamePlayer(id, betAmount, betSuit, gameId, userId);
        }

        public void Update(int betAmount, SuitType betSuit)
        {
            BetAmount = betAmount;
            BetSuit = betSuit;
        }
    }
}
