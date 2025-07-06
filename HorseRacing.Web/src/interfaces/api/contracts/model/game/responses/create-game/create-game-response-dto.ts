export interface CreateGameResponseDto {
    /** Код игры */
    GameId: string;
    /** Статус игры */
    Status: number;
    /** Наименование комнаты */
    Name: string;
}
