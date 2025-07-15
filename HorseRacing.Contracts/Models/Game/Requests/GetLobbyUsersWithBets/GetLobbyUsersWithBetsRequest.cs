using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.Game.Requests.GetLobbyUsersWithBets
{
    public class GetLobbyUsersWithBetsRequest : BaseRequest<BaseModelDto>
    {
        public GetLobbyUsersWithBetsRequest() : base() { }
        public GetLobbyUsersWithBetsRequest(BaseModelDto data) : base(data) { }
    }
}
