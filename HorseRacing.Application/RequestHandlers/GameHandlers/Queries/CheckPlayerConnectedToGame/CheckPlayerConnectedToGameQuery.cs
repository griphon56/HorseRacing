using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Queries.CheckPlayerConnectedToGame
{
    /// <summary>
    /// Команда проверки, подключен ли пользователь к игре 
    /// </summary>
    public class CheckPlayerConnectedToGameQuery : IRequest<ErrorOr<CheckPlayerConnectedToGameResult>>
    {
        /// <summary>
        /// Код пользователя
        /// </summary>
        public UserId UserId { get; set; }
        /// <summary>
        /// Код игры
        /// </summary>
        public GameId GameId { get; set; }

        public CheckPlayerConnectedToGameQuery(GameId gameId, UserId userId)
        {
            UserId = userId;
            GameId = gameId;
        }
    }
}
