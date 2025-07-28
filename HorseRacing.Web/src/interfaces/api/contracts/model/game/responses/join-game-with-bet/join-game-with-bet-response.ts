import type { BaseResponse } from '../../../../base/responses/base-response';
import type { JoinGameWithBetResponseDto } from './join-game-with-bet-response-dto';

export class JoinGameWithBetResponse implements BaseResponse<JoinGameWithBetResponseDto> {
    Data: JoinGameWithBetResponseDto;

    constructor(data?: JoinGameWithBetResponseDto) {
        this.Data = data || { IsLastPlayer: false };
    }
}
