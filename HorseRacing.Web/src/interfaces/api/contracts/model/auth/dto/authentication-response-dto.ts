import type { BaseDto } from "../../../base/dto/base-dto";

export interface AuthenticationResponseDto extends BaseDto {
    Id: string;
    Username: string;
    Token: string;
    FirstName: string;
    LastName: string;
    Email: string;
}