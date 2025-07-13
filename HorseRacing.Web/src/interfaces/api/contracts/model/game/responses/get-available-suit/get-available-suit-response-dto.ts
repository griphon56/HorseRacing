import type { BaseDto } from "~/interfaces/api/contracts/base/dto/base-dto";
import type { SuitType } from "../../enums/suit-type-enum";

export interface GetAvailableSuitResponseDto extends BaseDto {
    /** Код масти */
    Suit : SuitType;
    /** Название масти */
    Name: string;
}
