import type { BaseDto } from '~/interfaces/api/contracts/base/dto/base-dto';

export interface GetUserResponseDto extends BaseDto {
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
}
