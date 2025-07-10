using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.Game.Requests.JoinGame
{
    public class JoinGameRequest : BaseRequest<JoinGameRequestDto>
    {
        public JoinGameRequest() : base() { }
        public JoinGameRequest(JoinGameRequestDto data) : base(data) { }
    }
}
