using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.Game.Requests.GetGameResult
{
    public class GetGameResultRequest : BaseRequest<BaseModelDto>
    {
        public GetGameResultRequest() : base() { }
        public GetGameResultRequest(BaseModelDto data) : base(data) { }
    }
}
