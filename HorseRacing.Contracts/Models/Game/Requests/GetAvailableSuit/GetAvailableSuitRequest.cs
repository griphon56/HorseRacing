using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.Game.Requests.GetAvailableSuit
{
    public class GetAvailableSuitRequest : BaseRequest<BaseModelDto>
    {
        public GetAvailableSuitRequest() : base() { }
        public GetAvailableSuitRequest(BaseModelDto data) : base(data) { }
    }
}
