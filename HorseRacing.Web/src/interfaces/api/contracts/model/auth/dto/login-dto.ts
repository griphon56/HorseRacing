import type { BaseDto } from "../../../base/dto/base-dto";

export interface LoginDto extends BaseDto {
    UserName: string;
    Password: string;
}