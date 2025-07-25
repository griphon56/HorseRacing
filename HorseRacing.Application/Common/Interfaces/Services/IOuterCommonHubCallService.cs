﻿namespace HorseRacing.Application.Common.Interfaces.Services
{
    public interface IOuterCommonHubCallService
    {
        /// <summary>
        /// Отправка события что все пользователи присоединились к игре
        /// </summary>
        /// <param name="gameId">Код игры</param>
        Task AllPlayersJoinToGame(Guid gameId);
        /// <summary>
        /// Отправка события на клиент о том что нужно обновить список
        /// </summary>
        Task NotifyLobbyUpdate();
        /// <summary>
        /// Отправка события на клиент о том что нужно обновить список игроков в лобби
        /// </summary>
        /// <param name="gameId">Код игры</param>
        Task NotifyLobbyPlayersUpdate(Guid gameId);
        /// <summary>
        /// Отправка события на клиент о том что нужно обновить список доступных мастей
        /// </summary>
        /// <param name="gameId">Код игры</param>
        Task NotifyAvailableSuitsUpdate(Guid gameId);
    }
}
