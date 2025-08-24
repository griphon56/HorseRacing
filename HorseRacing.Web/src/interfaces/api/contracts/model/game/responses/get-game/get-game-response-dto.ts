import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';
import type { GameModeType } from '../../enums/game-mode-type-enum';
import type { StatusType } from '../../enums/status-type-enum';

export interface GetGameResponseDto extends BaseDto {
    /** Код игры */
    GameId: string;
    /** Статус игры */
    Status: StatusType;
    /** Режим игры */
    Mode: GameModeType;
    /** Наименование комнаты */
    Name: string;
    /** Предопределенная ставка при создании игры */
    DefaultBet?: number | null;
    /** Дата начала игры */
    DateStart: string;
    /** Дата окончания */
    DateEnd?: string;
}
