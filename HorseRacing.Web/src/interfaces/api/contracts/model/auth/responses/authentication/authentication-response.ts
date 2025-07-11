import type { BaseResponse } from "~/interfaces/api/contracts/base/responses/base-response";
import type { AuthenticationResponseDto } from "./authentication-response-dto";

export class AuthenticationResponse implements BaseResponse<AuthenticationResponseDto> {
    Data: AuthenticationResponseDto;

    constructor(data?: AuthenticationResponseDto) {
        this.Data = data || { Id: '', Username: '', Token: '', FirstName: '', LastName: '', Email: '' };
    }
}
