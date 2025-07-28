using HorseRacing.Contracts.Base.Responses;

namespace HorseRacing.Contracts.Models.Game.Responses.JoinGameWithBet
{
    public class JoinGameWithBetResponse : BaseResponse<JoinGameWithBetResponseDto>
    {
        public JoinGameWithBetResponse() : base() { }
        public JoinGameWithBetResponse(JoinGameWithBetResponseDto data) : base(data) { }
    }
}
