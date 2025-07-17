import type { BaseDto } from "~/interfaces/api/contracts/base/dto/base-dto";

export interface GetGameResponseDto extends BaseDto {
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
