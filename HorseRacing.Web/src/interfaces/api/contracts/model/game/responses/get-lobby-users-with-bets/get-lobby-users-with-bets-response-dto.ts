import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';
import type { GameUserDto } from '../../dto/game-user-dto';

export interface GetLobbyUsersWithBetsResponseDto extends BaseDto {
    /** Код игры */
    GameId: string;
    /** Наименование игры */
    GameName: string;
    /** Общий банк */
    TotalBank: number;
    /** Список игроков */
    Players: Array<GameUserDto> | [];
}
