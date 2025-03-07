using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetWaitingGames
{
    public class GetWaitingGamesQuery : IRequest<ErrorOr<GetWaitingGamesResult>> { }
}
