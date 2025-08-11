using HorseRacing.Api.Services;
using HorseRacing.Application.Common.Interfaces.Hubs;
using HorseRacing.Application.RequestHandlers.GameHandlers.Commands.StartGame;
using HorseRacing.Common;
using HorseRacing.Contracts.Models.Game.Responses.PlayGame;
using HorseRacing.Domain.Common.Models.Authentication.Configuration;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using MapsterMapper;
using MediatR;
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
        private readonly ReadyTrackerService _readyFlags;
        private readonly IMediator _mediator;
        private readonly CustomConnectionMapping<Guid> _connections;
        private readonly IMapper _mapper;

        public CommonServerHub(CustomConnectionMapping<Guid> connections, ReadyTrackerService readyFlags, IMediator mediator
            , IMapper mapper)
        {
            _connections = connections;
            _readyFlags = readyFlags;
            _mediator = mediator;
            _mapper = mapper;
        }

        public Task JoinToGame(string gameId)
        {
            var guid = Guid.Parse(gameId);
            _connections.Add(guid, Context.ConnectionId);
            return Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }

        public Task SubscribeGameListUpdate()
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, "GameListUpdate");
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

        public async Task RegisterReadyToStart(string gameId)
        {
            Guid.TryParse(Context.User.GetUserIdForHub(), out var userId);
            Guid.TryParse(gameId, out var gameIdGuid);

            _readyFlags.MarkReady(gameIdGuid, userId);

            if (_readyFlags.CountReady(gameIdGuid) == CommonSystemValues.NumberOfPlayers)
            {
                var result = await _mediator.Send(new PlayGameCommand()
                {
                    GameId = GameId.Create(gameIdGuid)
                });

                var response = new PlayGameResponse(_mapper.Map<PlayGameResponseDto>(result.Value));
                await Clients.Group(gameId).OnGameSimulationResult(response);

                _readyFlags.Clear(gameIdGuid);
            }
        }
    }
}
