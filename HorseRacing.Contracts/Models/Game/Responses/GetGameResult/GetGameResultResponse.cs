using HorseRacing.Contracts.Base.Responses;

namespace HorseRacing.Contracts.Models.Game.Responses.GetGameResult
{
    public class GetGameResultResponse : BaseListResponse<GetGameResultResponseDto>
    {
        public GetGameResultResponse() : base() { }

        public GetGameResultResponse(List<GetGameResultResponseDto> data) : base(data) { }
    }
}
