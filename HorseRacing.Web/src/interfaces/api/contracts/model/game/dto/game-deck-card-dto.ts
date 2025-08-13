import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';
import type { ZoneType } from '../enums/zone-type-enum';
import type { SuitType } from '../enums/suit-type-enum';
import type { RankType } from '../enums/rank-type-enum';

export interface GameDeckCardDto extends BaseDto {
    /** Масть */
    CardSuit: SuitType;
    /** Номинал */
    CardRank: RankType;
    /** Позиция в колоде */
    CardOrder: number;
    /** Нахождение в игре */
    Zone: ZoneType;
}
