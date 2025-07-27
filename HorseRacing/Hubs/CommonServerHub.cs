using HorseRacing.Application.Common.Interfaces.Hubs;
using HorseRacing.Domain.Common.Models.Authentication.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HorseRacing.Api.Hubs
{
    /// <summary>
    /// Общий класс-концентратор (хаб), необходим для работы с SignalR
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtAuthenticationModuleOption.SchemeName)]
    public class CommonServerHub : Hub<ICommonServerHub>
    {
        private readonly CustomConnectionMapping<Guid> _connections;

        public CommonServerHub(CustomConnectionMapping<Guid> connections)
        {
            _connections = connections;
        }

        public Task JoinToGame(string gameId)
        {
            var guid = Guid.Parse(gameId);
            _connections.Add(guid, Context.ConnectionId);
            return Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }

        public Task SubscribeToUpdateListLobby()
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, "LobbyViewersGroup");
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // Чистим ConnectionId из маппинга
            foreach (var kv in _connections.GetAllKeys())
            {
                _connections.Remove(kv, Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
