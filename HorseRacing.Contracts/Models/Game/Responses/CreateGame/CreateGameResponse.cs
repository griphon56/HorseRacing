using HorseRacing.Contracts.Base.Responses;

namespace HorseRacing.Contracts.Models.Game.Responses.CreateGame
{
    public class CreateGameResponse : BaseResponse<CreateGameResponseDto>
    {
        public CreateGameResponse() : base() { }

        public CreateGameResponse(CreateGameResponseDto data) : base(data) { }
    }
}
