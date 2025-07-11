import type { BaseResponse } from "~/interfaces/api/contracts/base/responses/base-response";
import type { GetWaitingGamesResponseDto } from "./get-waiting-games-response-dto";

export class GetWaitingGamesResponse implements BaseResponse<GetWaitingGamesResponseDto> {
    Data: GetWaitingGamesResponseDto;

    constructor(data?: GetWaitingGamesResponseDto) {
        this.Data = data || { Games: []};
    }
}
