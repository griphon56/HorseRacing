using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetLobbyUsersWithBets
{
    public class GetLobbyUsersWithBetsQueryHandler : IRequestHandler<GetLobbyUsersWithBetsQuery, ErrorOr<GetLobbyUsersWithBetsResult>>
    {
        private readonly ILogger<GetLobbyUsersWithBetsQueryHandler> _logger;
        private readonly IGameRepository _gameRepository;

        public GetLobbyUsersWithBetsQueryHandler(ILogger<GetLobbyUsersWithBetsQueryHandler> logger, IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<ErrorOr<GetLobbyUsersWithBetsResult>> Handle(GetLobbyUsersWithBetsQuery request, CancellationToken cancellationToken)
        {
            var result = await _gameRepository.GetLobbyUsersWithBets(request.GameId, cancellationToken);

            return new GetLobbyUsersWithBetsResult()
            {
                Data = result
            };
        }
    }
}
