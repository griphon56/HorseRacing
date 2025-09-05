import type { BaseResponse } from '~/interfaces/api/contracts/base/responses/base-response';
import type { GetUserResponseDto } from './get-user-response-dto';

export class GetUserResponse implements BaseResponse<GetUserResponseDto> {
    Data: GetUserResponseDto;

    constructor(data?: GetUserResponseDto) {
        this.Data = data || { FirstName: '', LastName: '', UserName: '', Phone: '', Email: '' };
    }
}
