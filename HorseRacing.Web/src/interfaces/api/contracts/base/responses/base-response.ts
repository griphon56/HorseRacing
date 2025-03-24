import type { BaseDto } from "../dto/base-dto";

export interface BaseResponse<T> {
    Data: T;
}

export interface BaseListResponse<T extends BaseDto> {
    DataValues: T[];
}