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

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.CreateGame
{
    public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, ErrorOr<CreateGameResult>>
    {
        private readonly ILogger<CreateGameCommandHandler> _logger;
        private readonly IGameRepository _gameRepository;
        private readonly IUserService _userService;
        private readonly IGameService _gameService;

        public CreateGameCommandHandler(IGameRepository gameRepository, ILogger<CreateGameCommandHandler> logger
            , IUserService userService, IGameService gameService)
        {
            _logger = logger;
            _gameRepository = gameRepository;
            _userService = userService;
            _gameService = gameService;
        }

        public async Task<ErrorOr<CreateGameResult>> Handle(CreateGameCommand command, CancellationToken cancellationToken)
        {
            var userView = await _userService.GetWorkingUser(cancellationToken: cancellationToken);
            Game game = Game.Create(GameId.CreateUnique(), command.Name, Domain.GameAggregate.Enums.StatusType.Wait, new EntityChangeInfo(DateTime.UtcNow, userView.Value!.UserId));

            int bet = command.BetAmount == 0 ? 10 : command.BetAmount;
            
            var suit = command.BetSuit == Domain.GameAggregate.Enums.SuitType.None 
                ? _gameService.GetRandomAvilableSuit(game).Result.Value
                : command.BetSuit;

            game.JoinPlayer(GamePlayer.Create(GamePlayerId.CreateUnique(), bet, suit, game.Id, userView.Value!.UserId));

            await _gameRepository.Add(game, cancellationToken);

            _logger.Log(LogLevel.Information, $"[{DateTime.UtcNow}]: CreateGameCommand: {game.Name} ({game.Id.Value})");

            return new CreateGameResult()
            {
                GameId = game.Id,
                Name = game.Name,
                Status = game.Status
            };
        }
    }
}
