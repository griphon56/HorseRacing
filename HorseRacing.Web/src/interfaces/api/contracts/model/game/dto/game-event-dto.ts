import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';
import type { GameEventType } from '../enums/game-event-type-enum';

export interface GameEventDto extends BaseDto {
    /** Порядковый номер шага (1,2,3,…) */
    Step: number;
    /** Тип события */
    EventType: GameEventType;
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
    EventDate: string;
}
