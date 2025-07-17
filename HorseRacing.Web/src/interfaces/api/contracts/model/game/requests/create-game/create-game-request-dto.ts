import type { BaseDto } from "~/interfaces/api/contracts/base/dto/base-dto";

export interface CreateGameRequestDto extends BaseDto{
    /** Идентификатор пользователя создавшего игру */
    UserId: string;
    /** Название игры */
    Name: string;
    /** Ставка пользователя создавшего игру */
    BetAmount: number;
    /** Масть лошади, которую выбрал пользователь при создании игры */
    BetSuit: number;
}
