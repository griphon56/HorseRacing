using HorseRacing.Contracts.Models.Game.Responses.PlayGame;

namespace HorseRacing.Application.Common.Interfaces.Hubs
{
    public interface ICommonServerHub
    {
        #region InvokeMethods (Client -> Server)
        /// <summary>
        /// Подписывает текущее соединение на SignalR‑группу по gameId
        /// и сохраняет ConnectionId в маппинге.
        /// </summary>
        /// <param name="gameId">Код игры</param>
        Task JoinToGame(string gameId);
        /// <summary>
        /// Подписывает текущее соединение на SignalR‑группу об обновлении списка игр
        /// </summary>
        Task SubscribeGameListUpdate();
        /// <summary>
        /// Клиент вызывает при переходе на страницу гонки
        /// </summary>
        /// <param name="gameId">Код игры</param>
        Task RegisterReadyToStart(string gameId);
        #endregion

        #region Events (Server -> Client)
        /// <summary>
        /// Событие о начале игры
        /// </summary>
        Task GoToRaceEvent();
        /// <summary>
        /// Обновление списка игр
        /// </summary>
        Task OnGameListUpdated();
        /// <summary>
        /// Событие обновления игроков лобби
        /// </summary>
        Task OnLobbyPlayerListUpdated();
        /// <summary>
        /// События обновления доступных мастей
        /// </summary>
        Task OnAvailableSuitsUpdated();
        /// <summary>
        /// Сервер шлёт всем готовый ответ симуляции
        /// </summary>
        /// <param name="data"></param>
        Task OnGameSimulationResult(PlayGameResponse data);
        #endregion
    }
}
