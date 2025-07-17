using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.CheckPlayerConnectedToGame
{
    public class CheckPlayerConnectedToGameQueryHandler : IRequestHandler<CheckPlayerConnectedToGameQuery, ErrorOr<CheckPlayerConnectedToGameResult>>
    {
        private readonly ILogger<CheckPlayerConnectedToGameQueryHandler> _logger;
        private readonly IGameRepository _gameRepository;

        public CheckPlayerConnectedToGameQueryHandler(ILogger<CheckPlayerConnectedToGameQueryHandler> logger, IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<ErrorOr<CheckPlayerConnectedToGameResult>> Handle(CheckPlayerConnectedToGameQuery request, CancellationToken cancellationToken)
        {
            var result = await _gameRepository.CheckPlayerConnectedToGame(request.GameId, request.UserId, cancellationToken);

            return new CheckPlayerConnectedToGameResult(result);
        }
    }
}
