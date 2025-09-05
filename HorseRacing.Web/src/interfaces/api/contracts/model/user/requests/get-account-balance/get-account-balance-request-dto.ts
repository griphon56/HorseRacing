import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';

export interface GetAccountBalanceRequestDto extends BaseDto {
    /** Идентификатор пользователя */
    UserId: string;
}
