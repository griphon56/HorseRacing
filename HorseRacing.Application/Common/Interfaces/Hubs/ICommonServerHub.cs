namespace HorseRacing.Application.Common.Interfaces.Hubs
{
    public interface ICommonServerHub
    {
        /// <summary>
        /// Подписывает текущее соединение на SignalR‑группу по gameId
        /// и сохраняет ConnectionId в маппинге.
        /// </summary>
        /// <param name="gameId">Код игры</param>
        Task JoinToGame(string gameId);
        /// <summary>
        /// Событие о начале игры
        /// </summary>
        Task StartGame();
        /// <summary>
        /// Подписка на обновление списка игр
        /// </summary>
        Task SubscribeToLobby();
        /// <summary>
        /// Отписка от обновления списка игр
        /// </summary>
        Task UnsubscribeFromLobby();
        /// <summary>
        /// Обновление списка игр
        /// </summary>
        Task UpdateListLobby();
    }
}
