import type { BaseDto } from "../../../base/dto/base-dto";

export interface LoginDto extends BaseDto {
    Username: string;
    Password: string;
}