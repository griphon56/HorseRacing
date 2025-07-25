﻿using HorseRacing.Domain.GameAggregate.ValueObjects;

namespace HorseRacing.Domain.GameAggregate.ReadOnlyModels
{
    public class LobbyUsersWithBetsView
    {
        /// <summary>
        /// Код игры
        /// </summary>
        public GameId GameId { get; set; }
        /// <summary>
        /// Наименование игры
        /// </summary>
        public string GameName { get; set; } = string.Empty;
        /// <summary>
        /// Список пользователей в лобби с их ставками
        /// </summary>
        public List<GameUserView> Players { get; set; } = new();
    }
}
