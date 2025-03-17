using ErrorOr;

namespace HorseRacing.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Game
        {
            /// <summary>
            /// Игра не найдена
            /// </summary>
            public static Error GameNotFound => Error.Conflict($"{nameof(GameAggregate.Game)}.{nameof(GameNotFound)}", "Игра не найдена.");

            /// <summary>
            /// Превышено количество игроков в комнате
            /// </summary>
            public static Error LimitPlayers => Error.Conflict($"{nameof(GameAggregate.Game)}.{nameof(LimitPlayers)}", "Превышено количество игроков в комнате");

            /// <summary>
            /// Выбранная вами масть занята. Выберите другую, пожалуйста.
            /// </summary>
            public static Error SuitHasAlreadyBeenChosen => Error.Conflict($"{nameof(GameAggregate.Game)}.{nameof(SuitHasAlreadyBeenChosen)}", "Выбранная Вами масть занята. Выберите другую, пожалуйста.");

            /// <summary>
            /// Недостаточно средств для ставки
            /// </summary>
            public static Error NotEnoughFundsToPlaceBet => Error.Conflict($"{nameof(GameAggregate.Game)}.{nameof(NotEnoughFundsToPlaceBet)}", "Недостаточно средств для ставки.");
        }
    }
}
