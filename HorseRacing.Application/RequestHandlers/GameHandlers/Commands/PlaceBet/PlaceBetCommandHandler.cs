using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Domain.Common.Errors;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.UserAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.PlaceBet
{
    public class PlaceBetCommandHandler : IRequestHandler<PlaceBetCommand, ErrorOr<Unit>>
    {
        private readonly ILogger<PlaceBetCommandHandler> _logger;
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;

        public PlaceBetCommandHandler(ILogger<PlaceBetCommandHandler> logger, IGameRepository gameRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<Unit>> Handle(PlaceBetCommand command, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetById(command.UserId, cancellationToken) is not User user)
            {
                return Errors.Authentication.UserNotFound;
            }

            if (await _gameRepository.GetById(command.GameId, cancellationToken, false) is not Game game)
            {
                return Errors.Game.GameNotFound;
            }

            if (game.GamePlayers.Where(x => x.BetSuit == command.BetSuit).Any())
            {
                return Errors.Game.SuitHasAlreadyBeenChosen;
            }

            game.PlaceBet(command.UserId, command.BetAmount, command.BetSuit);

            await _gameRepository.Update(game, cancellationToken);

            _logger.Log(LogLevel.Information, $"[{DateTime.UtcNow}]: PlaceBetCommand: {user.UserName} ({user.Id.Value}) bet: {command.BetAmount} suit: {command.BetSuit}");

            return Unit.Value;
        }
    }
}
