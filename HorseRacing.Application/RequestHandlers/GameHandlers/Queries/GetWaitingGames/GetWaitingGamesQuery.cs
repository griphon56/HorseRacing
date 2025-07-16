using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetWaitingGames
{
    /// <summary>
    /// Команда получения списка игр находящихся в ожидании игроков.
    /// </summary>
    public class GetWaitingGamesQuery : IRequest<ErrorOr<GetWaitingGamesResult>> { }
}
