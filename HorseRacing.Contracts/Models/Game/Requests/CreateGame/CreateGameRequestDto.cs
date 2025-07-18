﻿using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Game.Requests.CreateGame
{
    public class CreateGameRequestDto : BaseDto
    {
        /// <summary>
        /// Идентификатор пользователя создавшего игру
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Название игры
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Ставка пользователя создавшего игру
        /// </summary>
        public int BetAmount { get; set; } = 10;
        /// <summary>
        /// Масть лошади, 
        /// которую пользователь выбрал при создании игры
        /// </summary>
        public int BetSuit { get; set; } = 0;
    }
}
