using HorseRacing.Contracts.Base.Responses;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Responses
{
    public class CreateGameResponse : BaseResponse<CreateGameResponseDto>
    {
        public CreateGameResponse() : base() { }

        public CreateGameResponse(CreateGameResponseDto data) : base(data) { }
    }
}
