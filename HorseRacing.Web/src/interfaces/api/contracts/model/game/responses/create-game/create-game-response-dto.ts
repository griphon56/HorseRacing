import type { BaseDto } from "~/interfaces/api/contracts/base/dto/base-dto";

export interface CreateGameResponseDto extends BaseDto {
    /** Код игры */
    GameId: string;
    /** Статус игры */
    Status: number;
    /** Наименование комнаты */
    Name: string;
}
