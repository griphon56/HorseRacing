using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.GetAvailableSuit
{
    /// <summary>
    /// Команда на получение доступной масти
    /// </summary>
    public class GetAvailableSuitQuery : IRequest<ErrorOr<GetAvailableSuitResult>>
    {
        public required GameId GameId { get; set; }
    }
}
