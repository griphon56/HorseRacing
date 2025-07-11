import type { BaseListResponse } from "~/interfaces/api/contracts/base/responses/base-response";
import type { GetAvailableSuitResponseDto } from "./get-available-suit-response-dto";

export class GetAvailableSuitResponse implements BaseListResponse<GetAvailableSuitResponseDto> {
    DataValues: GetAvailableSuitResponseDto[];

    constructor(data?: GetAvailableSuitResponseDto[]) {
        this.DataValues = data || [];
    }
}
