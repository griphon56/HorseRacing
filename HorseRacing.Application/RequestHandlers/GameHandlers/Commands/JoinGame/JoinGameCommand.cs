using ErrorOr;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.JoinGame
{
    /// <summary>
    /// Команда подключения к игре
    /// </summary>
    public class JoinGameCommand : IRequest<ErrorOr<Unit>>
    {
        /// <summary>
        /// Код пользователя
        /// </summary>
        public required UserId UserId { get; set; }
        /// <summary>
        /// Код игры
        /// </summary>
        public required GameId GameId { get; set; }
    }
}
