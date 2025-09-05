import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';

export interface GetAccountBalanceResponseDto extends BaseDto {
    /** Код */
    UserId: string;
    /** Баланс счёта */
    Balance: number;
}
