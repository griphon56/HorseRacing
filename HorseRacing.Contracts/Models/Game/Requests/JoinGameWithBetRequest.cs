using HorseRacing.Contracts.Base.Requests;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Requests
{
    public class JoinGameWithBetRequest : BaseRequest<JoinGameWithBetRequestDto>
    {
        public JoinGameWithBetRequest() : base() { }
        public JoinGameWithBetRequest(JoinGameWithBetRequestDto data) : base(data) { }
    }
}
