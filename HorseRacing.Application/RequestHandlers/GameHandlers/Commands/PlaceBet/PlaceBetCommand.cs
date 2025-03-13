using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.PlaceBet
{
    public class PlaceBetCommand : IRequest<ErrorOr<PlaceBetResult>>
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
        /// Код игры
        /// </summary>
        public required GameId GameId { get; set; }
        /// <summary>
        /// Код пользователя
        /// </summary>
        public required UserId UserId { get; set; }
    }
}
