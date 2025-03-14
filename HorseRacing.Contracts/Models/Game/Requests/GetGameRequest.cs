using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.Game.Requests
{
    public class GetGameRequest : BaseRequest<BaseModelDto>
    {
        public GetGameRequest() : base() { }
        public GetGameRequest(BaseModelDto data) : base(data) { }
    }
}
