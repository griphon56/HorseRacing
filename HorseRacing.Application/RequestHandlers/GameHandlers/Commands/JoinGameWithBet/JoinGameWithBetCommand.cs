using ErrorOr;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.JoinGameWithBet
{
    /// <summary>
    /// Команда подключения к игре с ставкой
    /// </summary>
    public class JoinGameWithBetCommand : IRequest<ErrorOr<Unit>>
    {
        /// <summary>
        /// Ставка
        /// </summary>
        public int BetAmount { get; set; }
        /// <summary>
        /// Масть
        /// </summary>
        public SuitType BetSuit { get; set; }
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
