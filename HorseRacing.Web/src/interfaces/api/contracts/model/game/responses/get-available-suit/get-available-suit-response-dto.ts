import type { BaseDto } from "~/interfaces/api/contracts/base/dto/base-dto";

export interface GetAvailableSuitResponseDto extends BaseDto {
    /** Код масти */
    Suit : number;
    /** Название масти */
    Name: string;
}
