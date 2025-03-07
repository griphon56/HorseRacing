using HorseRacing.Contracts.Base.Responses;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Responses
{
    public class JoinGameResponse : BaseResponse<JoinGameResponseDto>
    {
        public JoinGameResponse() : base() { }

        public JoinGameResponse(JoinGameResponseDto data) : base(data) { }
    }
}