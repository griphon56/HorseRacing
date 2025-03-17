using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetWaitingGames
{
    public class GetWaitingGamesQueryHandler : IRequestHandler<GetWaitingGamesQuery, ErrorOr<GetWaitingGamesResult>>
    {
        private readonly ILogger<GetWaitingGamesQueryHandler> _logger;
        private readonly IGameRepository _gameRepository;

        public GetWaitingGamesQueryHandler(IGameRepository gameRepository, ILogger<GetWaitingGamesQueryHandler> logger)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }

        public async Task<ErrorOr<GetWaitingGamesResult>> Handle(GetWaitingGamesQuery request, CancellationToken cancellationToken)
        {
            var games = await _gameRepository.GetWaitingGames(cancellationToken);

            _logger.Log(LogLevel.Information, $"[{DateTime.UtcNow}]: GetWaitingGamesQuery: get {games.Count} item(s))");

            return new GetWaitingGamesResult()
            {
                Games = games
            };
        }
    }
}
