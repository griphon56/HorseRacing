﻿using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace HorseRacing.Domain.GameAggregate.Entities
{
    /// <summary>
    /// Сущность "Позиция лошади в игре"
    /// </summary>
    [Display(Description = "Позиция лошади в игре")]
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
        /// <summary>
        /// Метод создания лошади в игре
        /// </summary>
        /// <param name="id">Код лошади</param>
        /// <param name="gameId">Код игры</param>
        /// <param name="horseSuit">Масть лошади</param>
        /// <param name="position">Позиция</param>
        public static GameHorsePosition Create(GameHorsePositionId id, GameId gameId, SuitType horseSuit, int position)
        {
            return new GameHorsePosition(id, gameId, horseSuit, position);
        }
        /// <summary>
        /// Метод установки позиции лошади
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(int position)
        {
            Position = position;
        }
    }
}
