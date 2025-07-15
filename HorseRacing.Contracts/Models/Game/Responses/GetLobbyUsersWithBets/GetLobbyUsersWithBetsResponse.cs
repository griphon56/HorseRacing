using HorseRacing.Contracts.Base.Responses;

namespace HorseRacing.Contracts.Models.Game.Responses.GetLobbyUsersWithBets
{
    public class GetLobbyUsersWithBetsResponse : BaseResponse<GetLobbyUsersWithBetsResponseDto>
    {
        public GetLobbyUsersWithBetsResponse() : base() { }
        public GetLobbyUsersWithBetsResponse(GetLobbyUsersWithBetsResponseDto data) : base(data) { }
    }
}
