using HorseRacing.Contracts.Base.Responses;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Responses
{
    public class GetWaitingGamesResponse : BaseResponse<GetWaitingGamesResponseDto>
    {
        public GetWaitingGamesResponse() : base() { }

        public GetWaitingGamesResponse(GetWaitingGamesResponseDto data) : base(data) { }
    }
}
