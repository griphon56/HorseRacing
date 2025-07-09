import type { BaseRequest } from "~/interfaces/api/contracts/base/requests/base-request";
import type { JoinGameRequestDto } from "./join-game-request-dto";

export class JoinGameRequest implements BaseRequest<JoinGameRequestDto> {
    Data: JoinGameRequestDto;

    constructor(data?: JoinGameRequestDto) {
        this.Data = data || { UserId: '', GameId: '' };
    }
}
