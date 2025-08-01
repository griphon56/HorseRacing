using System.ComponentModel;

namespace HorseRacing.Domain.GameAggregate.Enums
{
    public enum GameEventType
    {
        [Description("Колода инициализирована")]
        DeckInitialized,
        [Description("Игра началась")]
        StartGame,
        [Description("Игра завершилась")]
        EndGame,
    }
}
