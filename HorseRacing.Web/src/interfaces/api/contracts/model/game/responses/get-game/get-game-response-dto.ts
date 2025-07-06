export interface GetGameResponseDto {
    /** Код игры */
    GameId: string;
    /** Статус игры */
    Status: number;
    /** Наименование комнаты */
    Name: string;
    /** Дата начала игры */
    DateStart: string;
    /** Дата окончания */
    DateEnd?: string;
}
