import type { BaseResponse } from '~/interfaces/api/contracts/base/responses/base-response';
import type { PlayGameResponseDto } from './play-game-response-dto';

export class PlayGameResponse implements BaseResponse<PlayGameResponseDto> {
    Data: PlayGameResponseDto;

    constructor(data?: PlayGameResponseDto) {
        this.Data = data || {
            GameId: '',
            Name: '',
            InitialDeck: [],
            HorseBets: [],
            Events: [],
        };
    }
}
