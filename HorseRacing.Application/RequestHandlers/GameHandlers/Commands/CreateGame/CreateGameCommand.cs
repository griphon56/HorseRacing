using ErrorOr;
using HorseRacing.Application.RequestHandlers.GameHandlers.Common;
using HorseRacing.Domain.GameAggregate.Enums;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Commands.CreateGame
{
    /// <summary>
    /// Команда создания игры
    /// </summary>
    public class CreateGameCommand : IRequest<ErrorOr<CreateGameResult>>
    {
        /// <summary>
        /// Код пользователя создавшего игру
        /// </summary>
        public required UserId UserId { get; set; }
        /// <summary>
        /// Название игры
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Ставка пользователя создавшего игру
        /// </summary>
        public int BetAmount { get; set; } = 10;
        /// <summary>
        /// Масть лошади, 
        /// которую пользователь выбрал при создании игры
        /// </summary>
        public SuitType BetSuit { get; set; } = 0;
    }
}
