import type { BaseResponse } from '../../../../base/responses/base-response';
import type { GetGameResponseDto } from './get-game-response-dto';

export class GetGameResponse implements BaseResponse<GetGameResponseDto> {
    Data: GetGameResponseDto;

    constructor(data?: GetGameResponseDto) {
        this.Data = data || {
            GameId: '',
            Name: '',
            Status: 0,
            Mode: 0,
            DefaultBet: 0,
            DateStart: '',
            DateEnd: '',
        };
    }
}
