using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetGame
{
    /// <summary>
    /// Команда получения информации о игре
    /// </summary>
    public class GetGameQuery : IRequest<ErrorOr<GetGameResult>>
    {
        /// <summary>
        /// Код игры
        /// </summary>
        public required GameId GameId { get; set; }
    }
}
