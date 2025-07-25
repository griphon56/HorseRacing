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

        public async Task AllPlayersJoinToGame(Guid gameId)
        {
            var groupName = gameId.ToString();
            await _hub.Clients.Group(groupName).StartGame();
        }

        public async Task NotifyLobbyUpdate()
        {
            await _hub.Clients.Group("LobbyViewersGroup").UpdateListLobby();
        }
    }
}
