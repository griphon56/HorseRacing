using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetGameResults
{
    public class GetGameResultQuery : IRequest<ErrorOr<GetGameResultsResult>>
    {
        public GameId GameId { get; set; }
    }
}
