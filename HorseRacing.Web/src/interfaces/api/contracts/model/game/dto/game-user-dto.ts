import type { BaseDto } from "../../../base/dto/base-dto";
import type { SuitType } from "../enums/suit-type-enum";

export interface GameUserDto extends BaseDto {
    /** Код пользователя */
    UserId: string;
    /** Полное имя игрока */
    FullName: string;
    /** Масть */
    BetSuit: SuitType;
    /** Ставка */
    BetAmount: number;
}
