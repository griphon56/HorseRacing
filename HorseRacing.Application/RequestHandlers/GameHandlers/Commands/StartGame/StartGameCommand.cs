using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.StartGame
{
    /// <summary>
    /// Команда начала игры
    /// </summary>
    public class StartGameCommand : IRequest<ErrorOr<StartGameResult>>
    {
        public required GameId GameId { get; set; }
    }
}
