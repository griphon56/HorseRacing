using HorseRacing.Contracts.Base.Responses;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Responses
{
    public class GetGameResultResponse : BaseResponse<GetGameResultResponseDto>
    {
        public GetGameResultResponse() : base() { }

        public GetGameResultResponse(GetGameResultResponseDto data) : base(data) { }
    }
}
