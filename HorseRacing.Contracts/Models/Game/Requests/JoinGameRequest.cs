using HorseRacing.Contracts.Base.Requests;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Requests
{
    public class JoinGameRequest : BaseRequest<JoinGameRequestDto>
    {
        public JoinGameRequest() : base() { }
        public JoinGameRequest(JoinGameRequestDto data) : base(data) { }
    }
}
