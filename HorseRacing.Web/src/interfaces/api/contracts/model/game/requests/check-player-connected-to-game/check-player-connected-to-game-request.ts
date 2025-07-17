import type { BaseRequest } from "../../../../base/requests/base-request";
import type { CheckPlayerConnectedToGameRequestDto } from "./check-player-connected-to-game-request-dto";

export class CheckPlayerConnectedToGameRequest implements BaseRequest<CheckPlayerConnectedToGameRequestDto> {
    Data: CheckPlayerConnectedToGameRequestDto;

    constructor(data?: CheckPlayerConnectedToGameRequestDto) {
        this.Data = data || { UserId: '', GameId: ''};
    }
}
