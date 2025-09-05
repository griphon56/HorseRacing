import type { BaseRequest } from '~/interfaces/api/contracts/base/requests/base-request';
import type { GetAccountBalanceRequestDto } from './get-account-balance-request-dto';

export class GetAccountBalanceRequest implements BaseRequest<GetAccountBalanceRequestDto> {
    Data: GetAccountBalanceRequestDto;

    constructor(data?: GetAccountBalanceRequestDto) {
        this.Data = data || { UserId: '' };
    }
}
