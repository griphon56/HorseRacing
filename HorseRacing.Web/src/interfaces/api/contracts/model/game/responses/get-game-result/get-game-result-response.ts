import type { BaseListResponse } from "~/interfaces/api/contracts/base/responses/base-response";
import type { GetGameResultResponseDto } from "./get-game-result-response-dto";

export class GetGameResultResponse implements BaseListResponse<GetGameResultResponseDto> {
    DataValues: GetGameResultResponseDto[];

    constructor(data?: GetGameResultResponseDto[]) {
        this.DataValues = data || [];
    }
}
