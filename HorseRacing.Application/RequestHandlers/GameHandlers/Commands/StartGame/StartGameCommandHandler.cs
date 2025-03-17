using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Domain.Common.Errors;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.Enums;
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
            _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) deck initialized");

            game.InitializeHorsePositions();
            _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) horse positions initialized");

            game.Update(game.Name, StatusType.InProgress, DateTime.UtcNow);

            await _gameRepository.Update(game, cancellationToken);
            _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) started");

            int i_block = 1;
            while (!game.IsGameFinished())
            {
                if (game.GameHorsePositions.Where(x=> x.Position>=i_block).Count() == 4)
                {
                    var cardBlock = game.GetCardFromTable();
                    _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) got card block {cardBlock.CardRank} {cardBlock.CardSuit}");

                    game.UpdateHorsePositionWithBlock(cardBlock);
                    _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) updated horse position with block");

                    i_block++;
                }

                var card = game.GetCardFromDeck();
                _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) got card {card.CardRank} {card.CardSuit}");

                game.UpdateHorsePosition(card);
                _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) updated horse position");

                await _gameRepository.Update(game, cancellationToken);
            }

            game.Update(game.Name, StatusType.Complete, game.DateEnd, DateTime.UtcNow);
            _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) is over");

            await _gameRepository.Update(game, cancellationToken);

            return Unit.Value;
        }
    }
}
