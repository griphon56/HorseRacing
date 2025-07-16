using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetGameResults
{
    /// <summary>
    /// Команда получения результатов игры по идентификатору игры.
    /// </summary>
    public class GetGameResultQuery : IRequest<ErrorOr<GetGameResultsResult>>
    {
        public required GameId GameId { get; set; }
    }
}
