using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.Entities;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using HorseRacing.Domain.Common.Errors;
using HorseRacing.Domain.UserAggregate;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.CreateGame
{
    public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, ErrorOr<CreateGameResult>>
    {
        private readonly ILogger<CreateGameCommandHandler> _logger;
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGameService _gameService;
        private readonly IOuterCommonHubCallService _hubCalls;

        public CreateGameCommandHandler(IGameRepository gameRepository, ILogger<CreateGameCommandHandler> logger
            , IUserRepository userRepository, IGameService gameService, IOuterCommonHubCallService hubCalls)
        {
            _logger = logger;
            _gameRepository = gameRepository;
            _userRepository = userRepository;
            _gameService = gameService;
            _hubCalls = hubCalls;
        }

        public async Task<ErrorOr<CreateGameResult>> Handle(CreateGameCommand command, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetById(command.UserId, cancellationToken) is not User user)
            {
                return Errors.Authentication.UserNotFound;
            }

            decimal bet = command.BetAmount == 0 ? 10 : command.BetAmount;

            if (user!.Account!.Balance - bet < 0)
            {
                return Errors.Game.NotEnoughFundsToPlaceBet;
            }

            Game game = Game.Create(GameId.CreateUnique(), command.Name, Domain.GameAggregate.Enums.StatusType.Wait
                , command.Mode, new EntityChangeInfo(DateTime.UtcNow, user.Id)
                , command.Mode == Domain.GameAggregate.Enums.GameModeType.Classic ? bet : null);

            var suit = command.BetSuit == Domain.GameAggregate.Enums.SuitType.None 
                ? _gameService.GetRandomAvilableSuit(game).Result.Value
                : command.BetSuit;

            game.JoinPlayer(GamePlayer.Create(GamePlayerId.CreateUnique(), bet, suit, game.Id, user.Id));

            await _gameRepository.Add(game, cancellationToken);

            _logger.Log(LogLevel.Information, $"[{DateTime.UtcNow}]: CreateGameCommand: {game.Name} ({game.Id.Value})");

            user.Account!.DebitBalance(bet);

            await _userRepository.Update(user, cancellationToken);

            _logger.Log(LogLevel.Information, $"[{DateTime.UtcNow}]: CreateGameCommand: {user.UserName} ({user.Id.Value}) debit {bet}");

            await _hubCalls.NotifyGameListUpdate();

            return new CreateGameResult()
            {
                GameId = game.Id,
                Name = game.Name,
                Status = game.Status
            };
        }
    }
}
