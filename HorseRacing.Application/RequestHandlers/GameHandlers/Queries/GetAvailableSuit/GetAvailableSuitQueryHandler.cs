using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Common;
using HorseRacing.Domain.Common.Errors;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ReadOnlyModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetAvailableSuit
{
    public class GetAvailableSuitQueryHandler : IRequestHandler<GetAvailableSuitQuery, ErrorOr<GetAvailableSuitResult>>
    {
        private readonly ILogger<GetAvailableSuitQueryHandler> _logger;
        private readonly IGameRepository _gameRepository;

        public GetAvailableSuitQueryHandler(ILogger<GetAvailableSuitQueryHandler> logger, IGameRepository gameRepository)
        {
            _logger = logger;
            _gameRepository = gameRepository;
        }

        public async Task<ErrorOr<GetAvailableSuitResult>> Handle(GetAvailableSuitQuery query, CancellationToken cancellationToken)
        {
            if (await _gameRepository.GetById(query.GameId, cancellationToken, true) is not Game game)
            {
                return Errors.Game.GameNotFound;
            }

            List<SuitType> exclude = new List<SuitType>() { SuitType.None };
            exclude.AddRange(game.GamePlayers.Select(x => x.BetSuit).ToList());

            List<SuitType> values = Enum.GetValues(typeof(SuitType)).Cast<SuitType>().ToList();
            var availableSuits = values.Except(exclude)
                .Select(x => new GameAvailableSuitView() { Name = x.GetDescription(), Suit = x }).ToList();

            _logger.Log(LogLevel.Information, $"GetAvailableSuitQuery: items:{availableSuits.Count}");

            return new GetAvailableSuitResult() { AvailableSuits = availableSuits };
        }
    }
}
