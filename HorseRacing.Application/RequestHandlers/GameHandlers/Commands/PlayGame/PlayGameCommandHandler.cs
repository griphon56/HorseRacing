using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.Common.Errors;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.StartGame
{
    public class PlayGameCommandHandler : IRequestHandler<PlayGameCommand, ErrorOr<PlayGameResult>>
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<PlayGameCommandHandler> _logger;

        public PlayGameCommandHandler(IGameRepository gameRepository, ILogger<PlayGameCommandHandler> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<ErrorOr<PlayGameResult>> Handle(PlayGameCommand command, CancellationToken cancellationToken)
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
                if (game.GameHorsePositions.Where(x => x.Position >= i_block).Count() == 4)
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

            game.Update(game.Name, StatusType.Complete, game.DateStart, DateTime.UtcNow);
            _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) is over");

            game.AddDomainEvent(new GameOverEvent(game));

            await _gameRepository.Update(game, cancellationToken);

            return Unit.Value;
        }
    }
}
