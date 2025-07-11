import type { BaseDto } from "~/interfaces/api/contracts/base/dto/base-dto";

export interface AuthenticationResponseDto extends BaseDto {
    /** Код пользователя */
    Id: string;
    /** Логин */
    Username: string;
    /** Токен доступа */
    Token: string;
    /** Имя */
    FirstName: string;
    /** Фамилия */
    LastName: string;
    /** Email */
    Email: string;
}
