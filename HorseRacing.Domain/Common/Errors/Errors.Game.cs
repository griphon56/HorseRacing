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
        }
    }
}
