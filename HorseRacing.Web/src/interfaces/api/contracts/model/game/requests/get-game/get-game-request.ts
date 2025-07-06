import type { BaseModelDto } from '~/interfaces/api/contracts/base/dto/base-model-dto';
import type { BaseRequest } from '../../../../base/requests/base-request';

export class GetGameRequest implements BaseRequest<BaseModelDto> {
    Data: BaseModelDto;

    constructor(data?: BaseModelDto) {
            this.Data = data || { Id: ''};
        }
}
