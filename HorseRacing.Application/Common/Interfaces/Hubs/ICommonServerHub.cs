namespace HorseRacing.Application.Common.Interfaces.Hubs
{
    public interface ICommonServerHub
    {
        #region InvokeMethods
        /// <summary>
        /// Подписывает текущее соединение на SignalR‑группу по gameId
        /// и сохраняет ConnectionId в маппинге.
        /// </summary>
        /// <param name="gameId">Код игры</param>
        Task JoinToGame(string gameId);
        /// <summary>
        /// Подписывает текущее соединение на SignalR‑группу об обновлении списка игр
        /// </summary>
        Task SubscribeToUpdateListLobby();
        #endregion

        #region Events
        /// <summary>
        /// Событие о начале игры
        /// </summary>
        Task StartGame();
        /// <summary>
        /// Обновление списка игр
        /// </summary>
        Task UpdateListLobby();
        /// <summary>
        /// Событие обновления игроков лобби
        /// </summary>
        Task UpdateLobbyPlayers();
        /// <summary>
        /// События обновления доступных мастей
        /// </summary>
        Task UpdateAvailableSuits();
        #endregion
    }
}
