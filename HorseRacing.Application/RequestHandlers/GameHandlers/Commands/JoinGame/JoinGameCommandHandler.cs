using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Domain.Common.Errors;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.Entities;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.JoinGame
{
    public class JoinGameCommandHandler : IRequestHandler<JoinGameCommand, ErrorOr<Unit>>
    {
        private readonly ILogger<JoinGameCommandHandler> _logger;
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;

        public JoinGameCommandHandler(ILogger<JoinGameCommandHandler> logger, IGameRepository gameRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<Unit>> Handle(JoinGameCommand command, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetById(command.UserId, cancellationToken) is not User user)
            {
                return Errors.Authentication.UserNotFound;
            }

            if (await _gameRepository.GetById(command.GameId, cancellationToken, false) is not Game game)
            {
                return Errors.Game.GameNotFound;
            }

            if (game.GamePlayers.Count >= 4)
            {
                return Errors.Game.LimitPlayers;
            }

            game.JoinPlayer(GamePlayer.Create(GamePlayerId.CreateUnique(), 0, Domain.GameAggregate.Enums.SuitType.None, command.GameId, command.UserId));

            await _gameRepository.Update(game, cancellationToken);

            _logger.Log(LogLevel.Information, $"JoinGameCommand: {user.UserName} ({user.Id.Value}) to: {game.Name}");

            return Unit.Value;
        }
    }
}
