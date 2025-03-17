using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetGameResults
{
    public class GetGameResultQueryHander : IRequestHandler<GetGameResultQuery, ErrorOr<GetGameResultsResult>>
    {
        private readonly ILogger<GetGameResultQueryHander> _logger;
        private readonly IGameRepository _gameRepository;

        public GetGameResultQueryHander(ILogger<GetGameResultQueryHander> logger, IGameRepository gameRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }

        public async Task<ErrorOr<GetGameResultsResult>> Handle(GetGameResultQuery query, CancellationToken cancellationToken)
        {
            var result = await _gameRepository.GetGameResults(query.GameId, cancellationToken);

            return new GetGameResultsResult() { GameResults = result };
        }
    }
}
