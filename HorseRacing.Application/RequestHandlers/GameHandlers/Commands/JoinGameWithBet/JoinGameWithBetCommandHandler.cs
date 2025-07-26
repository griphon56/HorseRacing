using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Common;
using HorseRacing.Domain.Common.Errors;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.Entities;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.JoinGameWithBet
{
    public class JoinGameWithBetCommandHandler : IRequestHandler<JoinGameWithBetCommand, ErrorOr<Unit>>
    {
        private readonly ILogger<JoinGameWithBetCommandHandler> _logger;
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGameService _gameService;
        private readonly IOuterCommonHubCallService _hubCalls;

        public JoinGameWithBetCommandHandler(ILogger<JoinGameWithBetCommandHandler> logger, IGameRepository gameRepository, IUserRepository userRepository, IGameService gameService, IOuterCommonHubCallService hubCalls)
        {
            _logger = logger;
            _gameRepository = gameRepository;
            _userRepository = userRepository;
            _gameService = gameService;
            _hubCalls = hubCalls;
        }

        public async Task<ErrorOr<Unit>> Handle(JoinGameWithBetCommand command, CancellationToken cancellationToken)
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

            int bet = command.BetAmount == 0 ? 10 : command.BetAmount;

            if (user!.Account!.Balance - bet < 0)
            {
                return Errors.Game.NotEnoughFundsToPlaceBet;
            }

            var suit = command.BetSuit == Domain.GameAggregate.Enums.SuitType.None
                ? _gameService.GetRandomAvilableSuit(game).Result.Value
                : command.BetSuit;

            game.JoinPlayer(GamePlayer.Create(GamePlayerId.CreateUnique(), bet, suit, command.GameId, command.UserId));

            await _gameRepository.Update(game, cancellationToken);

            _logger.Log(LogLevel.Information, $"[{DateTime.UtcNow}]: JoinGameWithBetCommand: {user.UserName} ({user.Id.Value}) to: {game.Name}, bet: {bet}, suit: {suit}");

            user.Account!.DebitBalance(bet);
            await _userRepository.Update(user, cancellationToken);
            _logger.Log(LogLevel.Information, $"[{DateTime.UtcNow}]: JoinGameWithBetCommand: {user.UserName} ({user.Id.Value}) debit {bet}");

            await _hubCalls.NotifyLobbyPlayersUpdate(command.GameId.Value);
            await _hubCalls.NotifyAvailableSuitsUpdate(command.GameId.Value);

            if (game.GamePlayers.Count == CommonSystemValues.NumberOfPlayers)
            {
                await _hubCalls.AllPlayersJoinToGame(command.GameId.Value);
            }

            return Unit.Value;
        }
    }
}
