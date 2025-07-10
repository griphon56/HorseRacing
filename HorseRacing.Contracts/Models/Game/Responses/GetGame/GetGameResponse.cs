using HorseRacing.Contracts.Base.Responses;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Responses.GetGame
{
    public class GetGameResponse : BaseResponse<GetGameResponseDto>
    {
        public GetGameResponse() : base() { }

        public GetGameResponse(GetGameResponseDto data) : base(data) { }
    }
}