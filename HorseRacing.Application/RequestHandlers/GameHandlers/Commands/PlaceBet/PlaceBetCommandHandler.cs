using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.Common.Errors;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.UserAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.PlaceBet
{
    public class PlaceBetCommandHandler : IRequestHandler<PlaceBetCommand, ErrorOr<PlaceBetResult>>
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

        public async Task<ErrorOr<PlaceBetResult>> Handle(PlaceBetCommand command, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetById(command.UserId, cancellationToken) is not User user)
            {
                return Errors.Authentication.NotFoundUser;
            }

            if (await _gameRepository.GetById(command.GameId, cancellationToken, false) is not Game game)
            {
                return Errors.Game.GameNotFound;
            }

            game.PlaceBet(command.UserId, command.BetAmount, command.BetSuit);

            await _gameRepository.Update(game);

            _logger.Log(LogLevel.Information, $"PlaceBetCommand: {user.UserName} ({user.Id.Value}) bet: {command.BetAmount} suit: {command.BetSuit}");

            return new PlaceBetResult();
        }
    }
}
