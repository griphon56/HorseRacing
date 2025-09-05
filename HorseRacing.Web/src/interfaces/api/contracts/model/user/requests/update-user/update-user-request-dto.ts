import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';

export interface UpdateUserRequestDto extends BaseDto {
    /** Код */
    UserId: string;
    /** Имя */
    FirstName: string;
    /** Фамилия */
    LastName: string;
    /** Логин */
    UserName: string;
    /** Телефон */
    Phone: string;
    /** Почта */
    Email: string;
    /** Пароль */
    Password: string;
}
