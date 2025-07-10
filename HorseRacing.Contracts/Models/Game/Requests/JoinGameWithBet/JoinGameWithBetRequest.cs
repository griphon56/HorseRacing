using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.Game.Requests.JoinGameWithBet
{
    public class JoinGameWithBetRequest : BaseRequest<JoinGameWithBetRequestDto>
    {
        public JoinGameWithBetRequest() : base() { }
        public JoinGameWithBetRequest(JoinGameWithBetRequestDto data) : base(data) { }
    }
}
