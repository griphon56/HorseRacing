using HorseRacing.Contracts.Base.Requests;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Requests
{
    public class GetGameResultRequest : BaseRequest<GetGameResultRequestDto>
    {
        public GetGameResultRequest() : base() { }
        public GetGameResultRequest(GetGameResultRequestDto data) : base(data) { }
    }
}
