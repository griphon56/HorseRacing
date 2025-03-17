using HorseRacing.Contracts.Base.Responses;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Responses
{
    public class GetGameResultResponse : BaseListResponse<GameResultDto>
    {
        public GetGameResultResponse() : base() { }

        public GetGameResultResponse(List<GameResultDto> data) : base(data) { }
    }
}
