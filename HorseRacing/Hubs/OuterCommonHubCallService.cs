using HorseRacing.Application.Common.Interfaces.Hubs;
using HorseRacing.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace HorseRacing.Api.Hubs
{
    public class OuterCommonHubCallService : IOuterCommonHubCallService
    {
        private readonly IHubContext<CommonServerHub, ICommonServerHub> _hub;
        private readonly CustomConnectionMapping<Guid> _connections;

        public OuterCommonHubCallService(
            IHubContext<CommonServerHub, ICommonServerHub> hub,
            CustomConnectionMapping<Guid> connections)
        {
            _hub = hub;
            _connections = connections;
        }

        /// <summary>
        /// Вызывается из бизнес‑логики, когда все игроки присоединились.
        /// Отправляет клиентам в группе и по отдельным ConnectionId команду StartGame.
        /// </summary>
        public async Task AllPlayersJoinToGame(Guid gameId)
        {
            var groupName = gameId.ToString();
            // 1) Шлём в SignalR‑группу
            await _hub.Clients.Group(groupName).StartGame();

            // 2) По всем сохранённым ConnectionId (на всякий случай)
            var connections = _connections.GetConnections(gameId);
            foreach (var connId in connections)
            {
                await _hub.Clients.Client(connId).StartGame();
            }
        }
    }
}
