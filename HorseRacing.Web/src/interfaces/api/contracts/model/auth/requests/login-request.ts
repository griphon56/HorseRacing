import type { BaseRequest } from "../../../base/requests/base-request";
import type { LoginDto } from "../dto/login-dto";

export class LoginRequest implements BaseRequest<LoginDto> {
    Data: LoginDto;

    constructor(data?: LoginDto) {
        this.Data = data || { Username: '', Password: '' };
    }
}