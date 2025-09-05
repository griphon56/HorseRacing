import type { BaseRequest } from '~/interfaces/api/contracts/base/requests/base-request';
import type { RegistrationRequestDto } from './registration-request-dto';

export class RegistrationRequest implements BaseRequest<RegistrationRequestDto> {
    Data: RegistrationRequestDto;

    constructor(data?: RegistrationRequestDto) {
        this.Data = data || {
            FirstName: '',
            LastName: '',
            UserName: '',
            Phone: '',
            Email: '',
            Password: '',
        };
    }
}
