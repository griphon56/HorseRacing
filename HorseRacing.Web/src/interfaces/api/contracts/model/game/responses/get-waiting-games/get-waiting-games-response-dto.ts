import type { BaseDto } from "~/interfaces/api/contracts/base/dto/base-dto";
import type { GameDto } from "../../dto/game-dto"

export interface GetWaitingGamesResponseDto extends BaseDto {
    /** Список игр */
    Games : Array<GameDto> | [];
}
