using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.Game.Requests
{
    public class GetGameResultRequest : BaseRequest<BaseModelDto>
    {
        public GetGameResultRequest() : base() { }
        public GetGameResultRequest(BaseModelDto data) : base(data) { }
    }
}
