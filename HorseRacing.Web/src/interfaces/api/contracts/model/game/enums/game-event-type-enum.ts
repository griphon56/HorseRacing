export enum GameEventType {
    /** Колода инициализирована */
    DeckInitialized = 0,
    /** Позиции лошадей инициализированы */
    HorsePositionsInitialized = 1,
    /** Игра началась */
    StartGame = 2,
    /** Открыта карта-преграда */
    ObstacleCardRevealed = 3,
    /** Лошадь отступила из-за преграды */
    HorseRetreatedByObstacle = 4,
    /** Получена карта из колоды */
    GetCardFromDeck = 5,
    /** Обновлена позиция лошади */
    UpdateHorsePosition = 6,
    /** Игра завершилась */
    EndGame = 7
}
