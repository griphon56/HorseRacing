using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.Common.Errors;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.Events;
using HorseRacing.Domain.GameAggregate.ReadOnlyModels;
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

            #region Deck initialize
            game.InitializeDeck();

            var deckBeforeGameStarts = game.GameDeckCards.Select(x => new GameDeckCardView(x.CardSuit, x.CardRank, x.CardOrder, x.Zone)).ToList();

            int step = 0;
            game.AddEventGame(command.GameId, step++, GameEventType.DeckInitialized);
            _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) deck initialized");
            #endregion

            #region Horse positions initialized
            game.InitializeHorsePositions();

            game.AddEventGame(command.GameId, step++, GameEventType.HorsePositionsInitialized);
            _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) horse positions initialized");

            #endregion

            #region Start game
            game.Update(game.Name, StatusType.InProgress, DateTime.UtcNow);

            game.AddEventGame(command.GameId, step++, GameEventType.StartGame);
            await _gameRepository.Update(game, cancellationToken);

            _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) started");
            #endregion

            int i_block = 1;
            while (!game.IsGameFinished())
            {
                if (game.GameHorsePositions.Where(x => x.Position >= i_block).Count() == 4)
                {
                    var cardBlock = game.GetCardFromTable();
                    game.AddEventGame(command.GameId, step++, GameEventType.ObstacleCardRevealed, cardBlock.CardSuit, cardBlock.CardRank, cardBlock.CardOrder);
                    _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) got card block {cardBlock.CardRank} {cardBlock.CardSuit}");

                    int horsePositionObstacle = game.UpdateHorsePositionWithBlock(cardBlock);
                    game.AddEventGame(command.GameId, step++, GameEventType.HorseRetreatedByObstacle, cardBlock.CardSuit, cardBlock.CardRank, null, cardBlock.CardSuit, horsePositionObstacle);
                    _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) updated horse position with block");

                    i_block++;
                }

                var card = game.GetCardFromDeck();
                game.AddEventGame(command.GameId, step++, GameEventType.GetCardFromDeck, card.CardSuit, card.CardRank, card.CardOrder);
                _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) got card {card.CardRank} {card.CardSuit}");

                int horsePosition = game.UpdateHorsePosition(card);
                game.AddEventGame(command.GameId, step++, GameEventType.UpdateHorsePosition, card.CardSuit, card.CardRank, null, card.CardSuit, horsePosition);
                _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) updated horse position");

                await _gameRepository.Update(game, cancellationToken);
            }

            #region End game
            game.AddEventGame(command.GameId, step++, GameEventType.EndGame);
            game.Update(game.Name, StatusType.Complete, game.DateStart, DateTime.UtcNow);
            _logger.LogInformation($"[{DateTime.UtcNow}]: Game {game.Name} ({game.Id.Value}) is over");
            #endregion

            game.AddDomainEvent(new GameOverEvent(game));

            await _gameRepository.Update(game, cancellationToken);

            var gameEventsResult = game.GameEvents
                .Select(x => new GameEventView(x.Step, x.EventType, x.CardSuit, x.CardRank, x.CardOrder, x.HorseSuit, x.Position, x.EventDate))
                .ToList();

            var horseBet = await _gameRepository.GetHorseBet(command.GameId, cancellationToken);

            return new PlayGameResult(game.Id, game.Name, deckBeforeGameStarts, horseBet, gameEventsResult);
        }
    }
}
