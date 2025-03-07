﻿using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Dtos
{
    public class JoinGameRequestDto : BaseDto
    {
        /// <summary>
        /// Код игры
        /// </summary>
        public Guid GameId { get; set; }
        /// <summary>
        /// Код пользователя
        /// </summary>
        public Guid UserId { get; set; }
    }
}
