import type { BaseRequest } from "~/interfaces/api/contracts/base/requests/base-request";
import type { JoinGameWithBetRequestDto } from "./join-game-with-bet-request-dto";

export class JoinGameWithBetRequest implements BaseRequest<JoinGameWithBetRequestDto> {
    Data: JoinGameWithBetRequestDto;

    constructor(data?: JoinGameWithBetRequestDto) {
        this.Data = data || { UserId: '', GameId: '', BetAmount: 0, BetSuit: 0 };
    }
}
