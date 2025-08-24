import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';

export interface GetGameResultResponseDto extends BaseDto {
    /** Место, которое заняла лошадь в игре */
    Place: number;
    /** Масть */
    BetSuit: number;
    /** Код игры */
    GameId: string;
    /** Код пользователя */
    UserId: string;
    /** Полное имя пользователя */
    FullName: string;
}
