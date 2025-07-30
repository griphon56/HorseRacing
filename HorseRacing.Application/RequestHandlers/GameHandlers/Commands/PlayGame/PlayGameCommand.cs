using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.StartGame
{
    /// <summary>
    /// Команда начала игры
    /// </summary>
    public class PlayGameCommand : IRequest<ErrorOr<PlayGameResult>>
    {
        public required GameId GameId { get; set; }
    }
}
