import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';
import type { SuitType } from '../../enums/suit-type-enum';

export interface GetGameResultResponseDto extends BaseDto {
    /** Место, которое заняла лошадь в игре */
    Place: number;
    /** Масть */
    BetSuit: SuitType;
    /** Код игры */
    GameId: string;
    /** Код пользователя */
    UserId: string;
    /** Полное имя пользователя */
    FullName: string;
}
