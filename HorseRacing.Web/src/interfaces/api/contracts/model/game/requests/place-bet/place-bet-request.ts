import type { BaseRequest } from "~/interfaces/api/contracts/base/requests/base-request";
import type { PlaceBetRequestDto } from "./place-bet-request-dto";

export class PlaceBetRequest implements BaseRequest<PlaceBetRequestDto> {
    Data: PlaceBetRequestDto;

    constructor(data?: PlaceBetRequestDto) {
        this.Data = data || { UserId: '', GameId: '', BetAmount: 0, BetSuit: 0 };
    }
}
