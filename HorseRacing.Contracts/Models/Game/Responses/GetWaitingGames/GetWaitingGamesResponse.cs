using HorseRacing.Contracts.Base.Responses;

namespace HorseRacing.Contracts.Models.Game.Responses.GetWaitingGames
{
    public class GetWaitingGamesResponse : BaseResponse<GetWaitingGamesResponseDto>
    {
        public GetWaitingGamesResponse() : base() { }

        public GetWaitingGamesResponse(GetWaitingGamesResponseDto data) : base(data) { }
    }
}
