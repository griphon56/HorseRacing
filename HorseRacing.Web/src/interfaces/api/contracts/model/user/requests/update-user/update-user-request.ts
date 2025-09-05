import type { BaseRequest } from '~/interfaces/api/contracts/base/requests/base-request';
import type { UpdateUserRequestDto } from './update-user-request-dto';

export class UpdateUserRequest implements BaseRequest<UpdateUserRequestDto> {
    Data: UpdateUserRequestDto;

    constructor(data?: UpdateUserRequestDto) {
        this.Data = data || {
            UserId: '',
            FirstName: '',
            LastName: '',
            UserName: '',
            Phone: '',
            Email: '',
            Password: '',
        };
    }
}
