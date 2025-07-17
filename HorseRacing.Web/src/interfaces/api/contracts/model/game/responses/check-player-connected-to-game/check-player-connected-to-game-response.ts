import type { BaseResponse } from '../../../../base/responses/base-response';
import type { CheckPlayerConnectedToGameResponseDto } from './check-player-connected-to-game-response-dto';

export class CheckPlayerConnectedToGameResponse implements BaseResponse<CheckPlayerConnectedToGameResponseDto> {
    Data: CheckPlayerConnectedToGameResponseDto;

    constructor(data?: CheckPlayerConnectedToGameResponseDto) {
        this.Data = data || { IsConnected: false };
    }
}
