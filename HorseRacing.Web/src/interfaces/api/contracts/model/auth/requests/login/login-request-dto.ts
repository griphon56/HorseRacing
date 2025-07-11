import type { BaseDto } from "~/interfaces/api/contracts/base/dto/base-dto";

export interface LoginRequestDto extends BaseDto {
    /** Логин */
    UserName: string;
    /** Пароль */
    Password: string;
}
