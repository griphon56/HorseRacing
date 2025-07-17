import type { BaseDto } from "~/interfaces/api/contracts/base/dto/base-dto";

export interface JoinGameWithBetRequestDto extends BaseDto {
    /** Идентификатор игры */
    GameId: string;
    /** Идентификатор пользователя */
    UserId: string;
    /** Ставка пользователя */
    BetAmount: number;
    /** Масть лошади, которую выбрал пользователь */
    BetSuit: number;
}
