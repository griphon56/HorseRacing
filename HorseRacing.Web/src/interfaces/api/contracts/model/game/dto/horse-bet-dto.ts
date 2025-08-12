import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';
import type { SuitType } from '../enums/suit-type-enum';

export interface HorseBetDto extends BaseDto {
    /** Масть */
    BetSuit: SuitType;
    /** Сумма ставки */
    BetAmount: number;
    /** Код пользователя */
    UserId: string;
    /** ФИО (логин) */
    FullName: string;
}
