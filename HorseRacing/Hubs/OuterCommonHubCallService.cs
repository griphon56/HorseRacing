using ErrorOr;
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

        public async Task NotifyStartGame(Guid gameId)
        {
            await _hub.Clients.Group(gameId.ToString()).GoToRaceEvent();
        }

        public async Task NotifyGameListUpdate()
        {
            await _hub.Clients.Group("GameListUpdate").OnGameListUpdated();
        }

        public async Task NotifyLobbyPlayerListUpdate(Guid gameId)
        {
            await _hub.Clients.Group(gameId.ToString()).OnLobbyPlayerListUpdated();
        }

        public async Task NotifyAvailableSuitsUpdate(Guid gameId)
        {
            await _hub.Clients.Group(gameId.ToString()).OnAvailableSuitsUpdated();
        }
    }
}
