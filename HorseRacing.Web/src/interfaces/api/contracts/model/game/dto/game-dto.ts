import type { BaseDto } from "../../../base/dto/base-dto";

export interface GameDto extends BaseDto {
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
