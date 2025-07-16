import type { BaseResponse } from "~/interfaces/api/contracts/base/responses/base-response";
import type { GetLobbyUsersWithBetsResponseDto } from "./get-lobby-users-with-bets-response-dto";

export class GetLobbyUsersWithBetsResponse implements BaseResponse<GetLobbyUsersWithBetsResponseDto> {
    Data: GetLobbyUsersWithBetsResponseDto;

    constructor(data?: GetLobbyUsersWithBetsResponseDto) {
        this.Data = data || { GameId: '', GameName: '', Players: []};
    }
}
