import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';
import type { GameEventType } from '../enums/game-event-type-enum';
import type { SuitType } from '../enums/suit-type-enum';
import type { RankType } from '../enums/rank-type-enum';

export interface GameEventDto extends BaseDto {
    /** Порядковый номер шага (1,2,3,…) */
    Step: number;
    /** Тип события */
    EventType: GameEventType;
    /** Масть карты */
    CardSuit?: SuitType | null;
    /** Номинал карты */
    CardRank?: RankType | null;
    /** Позиция карты в колоде */
    CardOrder?: number | null;
    /** Масть лошади */
    HorseSuit?: SuitType | null;
    /** Позиция лошади */
    Position?: number | null;
    /** Дата события */
    EventDate: string;
}
