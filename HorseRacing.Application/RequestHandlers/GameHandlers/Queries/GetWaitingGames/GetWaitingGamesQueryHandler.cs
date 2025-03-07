using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetWaitingGames
{
    public class GetWaitingGamesQueryHandler : IRequestHandler<GetWaitingGamesQuery, ErrorOr<GetWaitingGamesResult>>
    {
        public Task<ErrorOr<GetWaitingGamesResult>> Handle(GetWaitingGamesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
