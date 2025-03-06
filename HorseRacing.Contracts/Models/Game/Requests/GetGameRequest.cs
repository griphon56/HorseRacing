using HorseRacing.Contracts.Base.Requests;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Requests
{
    public class GetGameRequest : BaseRequest<GetGameRequestDto>
    {
        public GetGameRequest() : base() { }
        public GetGameRequest(GetGameRequestDto data) : base(data) { }
    }
}
