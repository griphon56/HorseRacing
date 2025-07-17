import type { BaseDto } from "~/interfaces/api/contracts/base/dto/base-dto";

export interface CheckPlayerConnectedToGameRequestDto extends BaseDto  {
    /** Идентификатор пользователя */
    UserId: string;
    /** Код игры */
    GameId: string;
}
