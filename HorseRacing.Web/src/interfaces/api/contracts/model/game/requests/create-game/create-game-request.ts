import type { BaseRequest } from "../../../../base/requests/base-request";
import type { CreateGameRequestDto } from './create-game-request-dto';

export class CreateGameRequest implements BaseRequest<CreateGameRequestDto> {
    Data: CreateGameRequestDto;

    constructor(data?: CreateGameRequestDto) {
        this.Data = data || { UserId: '', Name: '', BetAmount: 0, BetSuit: 0 };
    }
}
