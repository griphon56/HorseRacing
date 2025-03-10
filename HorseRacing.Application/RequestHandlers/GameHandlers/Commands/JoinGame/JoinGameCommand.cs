using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.JoinGame
{
    /// <summary>
    /// Команда подключения к игре
    /// </summary>
    public class JoinGameCommand : IRequest<ErrorOr<JoinGameResult>>
    {
        /// <summary>
        /// Код пользователя
        /// </summary>
        public UserId UserId { get; set; }
        /// <summary>
        /// Код игры
        /// </summary>
        public GameId GameId { get; set; }
    }
}
