import type { BaseRequest } from "~/interfaces/api/contracts/base/requests/base-request";
import type { LoginRequestDto } from "./login-request-dto";

export class LoginRequest implements BaseRequest<LoginRequestDto> {
    Data: LoginRequestDto;

    constructor(data?: LoginRequestDto) {
        this.Data = data || { UserName: '', Password: '' };
    }
}
