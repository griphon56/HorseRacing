using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Domain.Common.Errors;
using HorseRacing.Domain.GameAggregate.Entities;
using HorseRacing.Domain.GameAggregate.Events;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HorseRacing.Application.EventHandlers.Game
{
    public class GameOverEventHandler : INotificationHandler<GameOverEvent>
    {
        private readonly ILogger<GameOverEventHandler> _logger;
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;

        public GameOverEventHandler(ILogger<GameOverEventHandler> logger, IGameRepository gameRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
            _userRepository = userRepository;
        }

        public async Task Handle(GameOverEvent gameOver, CancellationToken cancellationToken)
        {
            var game = gameOver.game;

            if (game.GamePlayers is not null && game.GamePlayers.Any())
            {
                var winHorse = game.GameHorsePositions.Where(x => x.Position == 7).FirstOrDefault();
                var winPlayer = game.GamePlayers.Where(x => x.BetSuit == winHorse!.HorseSuit).FirstOrDefault();

                int winAmount = game.GamePlayers.Sum(player => player.BetAmount);

                var user = await _userRepository.GetById(winPlayer!.UserId, cancellationToken);
                if (user is null)
                {
                    _logger.LogError($"[{DateTime.UtcNow}]: Ошибки на уровне GameOverEventHandler = {Errors.Authentication.UserNotFound}");
                }

                user!.Account!.CreditBalance(winAmount);

                await _userRepository.Update(user, cancellationToken);
                _logger.LogInformation($"[{DateTime.UtcNow}]: GameOverEventHandler: {user.UserName} ({user.Id.Value}), credit balance +{winAmount}");

                List<GameResult> gameResults = new List<GameResult>();
                foreach (var player in game.GamePlayers)
                {
                    int positionHorse = game.GameHorsePositions.Where(x => x.HorseSuit == player.BetSuit)
                        .Select(x => x.Position).FirstOrDefault();

                    gameResults.Add(GameResult.Create(GameResultId.CreateUnique(), positionHorse, player.BetSuit, game.Id, player.UserId));
                }

                game.AddGameResult(gameResults);

                await _gameRepository.Update(game, cancellationToken);
                _logger.LogInformation($"[{DateTime.UtcNow}]: GameOverEventHandler: record of game result");
            }
        }
    }
}
