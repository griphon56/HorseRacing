using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Domain.Common.Errors;
using HorseRacing.Domain.GameAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.StartGame
{
    public class StartGameCommandHandler : IRequestHandler<StartGameCommand, ErrorOr<Unit>>
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<StartGameCommandHandler> _logger;

        public StartGameCommandHandler(IGameRepository gameRepository, ILogger<StartGameCommandHandler> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<ErrorOr<Unit>> Handle(StartGameCommand command, CancellationToken cancellationToken)
        {
            if (await _gameRepository.GetById(command.GameId, cancellationToken, false) is not Game game)
            {
                return Errors.Game.GameNotFound;
            }

            game.InitializeDeck();
            _logger.LogInformation($"Game {game.Name} ({game.Id.Value}) deck initialized");

            game.InitializeHorsePositions();
            _logger.LogInformation($"Game {game.Name} ({game.Id.Value}) horse positions initialized");

            await _gameRepository.Update(game, cancellationToken);
            _logger.LogInformation($"Game {game.Name} ({game.Id.Value}) started");

            while(!game.IsGameFinished())
            {
                var card = game.GetCardFromDeck();
                _logger.LogInformation($"Game {game.Name} ({game.Id.Value}) got card {card.CardRank} {card.CardSuit}");

                game.UpdateHorsePosition(card);
                _logger.LogInformation($"Game {game.Name} ({game.Id.Value}) updated horse position");

                await _gameRepository.Update(game, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
