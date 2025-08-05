import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';

export interface GameEventDto extends BaseDto {
    /** Порядковый номер шага (1,2,3,…) */
    Step: number;
    /** Тип события */
    EventType: number;
    /** Масть карты */
    CardSuit?: number | null;
    /** Номинал карты */
    CardRank?: number | null;
    /** Позиция карты в колоде */
    CardOrder?: number | null;
    /** Масть лошади */
    HorseSuit?: number | null;
    /** Позиция лошади */
    Position?: number | null;
    /** Дата события */
    EventDate: string; // ISO string (DateTime)
}
