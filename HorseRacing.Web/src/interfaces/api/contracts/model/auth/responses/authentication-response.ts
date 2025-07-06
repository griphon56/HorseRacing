import type { BaseResponse } from "../../base/responses/base-response";
import type { AuthenticationResponseDto } from "../auth/dto/authentication-response-dto";

export class AuthenticationResponse implements BaseResponse<AuthenticationResponseDto> {
    Data: AuthenticationResponseDto;

    constructor(data?: AuthenticationResponseDto) {
        this.Data = data || { Id: '', Username: '', Token: '', FirstName: '', LastName: '', Email: '' };
    }
}