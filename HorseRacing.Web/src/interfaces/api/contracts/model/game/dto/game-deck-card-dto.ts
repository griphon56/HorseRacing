import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';

export interface GameDeckCardDto extends BaseDto {
    /** Масть */
    CardSuit: number;
    /** Номинал */
    CardRank: number;
    /** Позиция в колоде */
    CardOrder: number;
    /** Нахождение в игре */
    Zone: number;
}
