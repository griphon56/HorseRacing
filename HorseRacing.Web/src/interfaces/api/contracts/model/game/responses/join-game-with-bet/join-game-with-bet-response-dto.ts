import type { BaseDto } from "~/interfaces/api/contracts/base/dto/base-dto";

export interface JoinGameWithBetResponseDto extends BaseDto {
    /** Признак подключения последнего игрока */
    IsLastPlayer : boolean;
}
