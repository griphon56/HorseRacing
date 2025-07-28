import type { BaseDto } from "../../../base/dto/base-dto";

export interface UserProfileDto extends BaseDto {
    Id: string;
    UserName: string;
    FirstName: string;
    LastName: string;
    Email: string;
}
