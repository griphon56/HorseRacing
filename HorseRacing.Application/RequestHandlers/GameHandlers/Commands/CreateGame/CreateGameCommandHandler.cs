using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.GameAggregate;
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

        public CreateGameCommandHandler(IGameRepository gameRepository, ILogger<CreateGameCommandHandler> logger
            , IUserService userService)
        {
            _logger = logger;
            _gameRepository = gameRepository;
            _userService = userService;
        }

        public async Task<ErrorOr<CreateGameResult>> Handle(CreateGameCommand command, CancellationToken cancellationToken)
        {
            var userView = await _userService.GetWorkingUser();
            Game game = Game.Create(GameId.CreateUnique(), command.Name, Domain.GameAggregate.Enums.StatusType.Wait, new EntityChangeInfo(DateTime.UtcNow, userView.Value!.UserId));

            await _gameRepository.Add(game);

            return new CreateGameResult()
            {
                GameId = game.Id,
                Name = game.Name,
                Status = game.Status
            };
        }
    }
}
