export interface GameDto {
    /** Код игры */
    GameId: string;
    /** Статус игры */
    Status: string;
    /** Наименование комнаты */
    Name: string;
    /** Дата начала игры */
    DateStart: string;
    /** Дата окончания */
    DateEnd?: string;
}
