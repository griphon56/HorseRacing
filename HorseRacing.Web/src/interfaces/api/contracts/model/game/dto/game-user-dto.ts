import type { SuitType } from "../enums/suit-type-enum";

export interface GameUserDto {
    /** Код пользователя */
    UserId: string;
    /** Полное имя игрока */
    FullName: string;
    /** Масть */
    BetSuit: SuitType;
    /** Ставка */
    BetAmount: number;
}
