using HorseRacing.Contracts.Base.Responses;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Responses
{
    public class StartGameResponse : BaseResponse<StartGameResponseDto>
    {
        public StartGameResponse() : base() { }

        public StartGameResponse(StartGameResponseDto data) : base(data) { }
    }
}
