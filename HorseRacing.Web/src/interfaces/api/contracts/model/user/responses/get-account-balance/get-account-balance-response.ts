import type { BaseResponse } from '~/interfaces/api/contracts/base/responses/base-response';
import type { GetAccountBalanceResponseDto } from './get-account-balance-response-dto';

export class GetAccountBalanceResponse implements BaseResponse<GetAccountBalanceResponseDto> {
    Data: GetAccountBalanceResponseDto;

    constructor(data?: GetAccountBalanceResponseDto) {
        this.Data = data || { UserId: '', Balance: 0 };
    }
}
