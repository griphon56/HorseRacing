using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
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

        public CreateGameCommandHandler(IGameRepository gameRepository, ILogger<CreateGameCommandHandler> logger)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }

        public async Task<ErrorOr<CreateGameResult>> Handle(CreateGameCommand command, CancellationToken cancellationToken)
        {
            var dt_now = DateTime.UtcNow;
            Game game = Game.Create(GameId.CreateUnique(), command.Name, Domain.GameAggregate.Enums.StatusType.Wait, dt_now, null, new EntityChangeInfo(dt_now));

            await _gameRepository.Add(game);

            return new CreateGameResult()
            {
                GameId = game.Id,
                DateEnd = null,
                DateStart = dt_now,
                Name = game.Name,
                Status = game.Status
            };
        }
    }
}
