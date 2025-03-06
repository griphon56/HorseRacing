using HorseRacing.Contracts.Base.Requests;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Requests
{
    public class CreateGameRequest : BaseRequest<CreateGameRequestDto>
    {
        public CreateGameRequest() : base() { }
        public CreateGameRequest(CreateGameRequestDto data) : base(data) { }
    }
}
