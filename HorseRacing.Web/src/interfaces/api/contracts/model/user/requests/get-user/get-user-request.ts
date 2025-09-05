import type { BaseRequest } from '~/interfaces/api/contracts/base/requests/base-request';
import type { GetUserRequestDto } from './get-user-request-dto';

export class GetUserRequest implements BaseRequest<GetUserRequestDto> {
    Data: GetUserRequestDto;

    constructor(data?: GetUserRequestDto) {
        this.Data = data || { UserId: '' };
    }
}
