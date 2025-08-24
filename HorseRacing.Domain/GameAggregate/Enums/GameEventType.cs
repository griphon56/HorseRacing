using System.ComponentModel;

namespace HorseRacing.Domain.GameAggregate.Enums
{
    public enum GameEventType
    {
        [Description("Колода инициализирована")]
        DeckInitialized,
        [Description("Позиции лошадей инициализированы")]
        HorsePositionsInitialized,
        [Description("Игра началась")]
        StartGame,
        [Description("Открыта карта-преграда")]
        ObstacleCardRevealed,
        [Description("Лошадь отступила из-за преграды")]
        HorseRetreatedByObstacle,
        [Description("Получена карта из колоды")]
        GetCardFromDeck,
        [Description("Обновлена позиция лошади")]
        UpdateHorsePosition,
        [Description("Игра завершилась")]
        EndGame,
        [Description("Лошадь финишировала")]
        HorseFinished
    }
}
