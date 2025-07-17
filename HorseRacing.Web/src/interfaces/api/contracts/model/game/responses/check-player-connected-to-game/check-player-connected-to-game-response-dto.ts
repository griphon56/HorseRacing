import type { BaseDto } from "~/interfaces/api/contracts/base/dto/base-dto";

export interface CheckPlayerConnectedToGameResponseDto extends BaseDto  {
    /** Признак подключения пользователя к игре */
    IsConnected: boolean;
}
