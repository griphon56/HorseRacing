using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.Game.Requests.CreateGame
{
    public class CreateGameRequest : BaseRequest<CreateGameRequestDto>
    {
        public CreateGameRequest() : base() { }
        public CreateGameRequest(CreateGameRequestDto data) : base(data) { }
    }
}
