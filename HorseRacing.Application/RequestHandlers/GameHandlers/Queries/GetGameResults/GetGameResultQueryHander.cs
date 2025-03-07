using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetGameResults
{
    public class GetGameResultQueryHander : IRequestHandler<GetGameResultQuery, ErrorOr<GetGameResultsResult>>
    {
        public Task<ErrorOr<GetGameResultsResult>> Handle(GetGameResultQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
