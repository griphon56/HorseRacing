import type { BaseResponse } from '../../../../base/responses/base-response';
import type { CreateGameResponseDto } from './create-game-response-dto';

export class CreateGameResponse implements BaseResponse<CreateGameResponseDto> {
    Data: CreateGameResponseDto;

    constructor(data?: CreateGameResponseDto) {
        this.Data = data || { GameId: '', Name: '', Status: 0 };
    }
}
