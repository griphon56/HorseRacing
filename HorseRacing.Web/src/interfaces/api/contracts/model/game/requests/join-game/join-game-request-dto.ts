import type { BaseDto } from "~/interfaces/api/contracts/base/dto/base-dto";

export interface JoinGameRequestDto extends BaseDto {
    /** Идентификатор игры */
    GameId: string;
    /** Идентификатор пользователя */
    UserId: string;
}
