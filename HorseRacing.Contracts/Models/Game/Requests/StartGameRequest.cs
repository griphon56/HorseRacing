using HorseRacing.Contracts.Base.Requests;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Requests
{
    public class StartGameRequest : BaseRequest<StartGameRequestDto>
    {
        public StartGameRequest() : base() { }
        public StartGameRequest(StartGameRequestDto data) : base(data) { }
    }
}
