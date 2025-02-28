using System.ComponentModel;

namespace HorseRacing.Domain.GameAggregate.Enums
{
    public enum GameEventType
    {
        [Description("Игра началась")]
        StartGame,
        [Description("Игра завершилась")]
        EndGame,
    }
}
