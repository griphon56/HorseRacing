using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ValueObjects;

namespace HorseRacing.Domain.GameAggregate.Entities
{
    /// <summary>
    /// Сущность "Позиция лошади в игре"
    /// </summary>
    public class GameHorsePosition : EntityGuid<GameHorsePositionId>
    {
        /// <summary>
        /// Лошадь в игре
        /// </summary>
        public SuitType HorseSuit { get; private set; }
        /// <summary>
        /// Позиция лошади в игре
        /// </summary>
        public int Position { get; private set; }
        /// <summary>
        /// Код игры
        /// </summary>
        public GameId GameId { get; private set; }

        private GameHorsePosition() : base(GameHorsePositionId.CreateUnique()) { }

        private GameHorsePosition(GameHorsePositionId id, GameId gameId, SuitType horseSuit, int position)
            : base(id ?? GameHorsePositionId.CreateUnique())
        {
            GameId = gameId;
            HorseSuit = horseSuit;
            Position = position;
        }

        public static GameHorsePosition Create(GameHorsePositionId id, GameId gameId, SuitType horseSuit, int position)
        {
            return new GameHorsePosition(id, gameId, horseSuit, position);
        }
    }
}
