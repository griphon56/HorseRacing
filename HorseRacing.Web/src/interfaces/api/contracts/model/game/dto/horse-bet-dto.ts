import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';

export interface HorseBetDto extends BaseDto {
    /** Масть */
    BetSuit: number;
    /** Сумма ставки */
    BetAmount: number;
    /** Код пользователя */
    UserId: string;
    /** ФИО (логин) */
    FullName: string;
}
