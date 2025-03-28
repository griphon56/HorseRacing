import type { BaseDto } from "../../../base/dto/base-dto";

export interface UserProfileDto extends BaseDto {
    Id: string;
    Username: string;
    FirstName: string;
    LastName: string;
    Email: string;
}