using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using HorseRacing.Domain.Common.Errors;
using MapsterMapper;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetGame
{
    public class GetGameQueryHandler : IRequestHandler<GetGameQuery, ErrorOr<GetGameResult>>
    {
        private readonly ILogger<GetGameQueryHandler> _logger;
        private readonly IGameRepository _gameRepository;
        private readonly IMapper _mapper;

        public GetGameQueryHandler(IGameRepository gameRepository, ILogger<GetGameQueryHandler> logger, IMapper mapper)
        {
            _logger = logger;
            _gameRepository = gameRepository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<GetGameResult>> Handle(GetGameQuery query, CancellationToken cancellationToken)
        {
            var game = await _gameRepository.GetById(query.GameId, cancellationToken);
            if (game is null)
                return Errors.Game.GameNotFound;

            _logger.Log(LogLevel.Information, $"[{DateTime.UtcNow}]: GetGameQuery: {game.Name} ({game.Id.Value})");

            return _mapper.Map<GetGameResult>(game);
        }
    }
}
