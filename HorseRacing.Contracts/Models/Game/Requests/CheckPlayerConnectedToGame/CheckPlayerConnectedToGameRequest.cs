using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.Game.Requests.CheckPlayerConnectedToGame
{
    public class CheckPlayerConnectedToGameRequest : BaseRequest<CheckPlayerConnectedToGameRequestDto>
    {
        public CheckPlayerConnectedToGameRequest() : base() { }
        public CheckPlayerConnectedToGameRequest(CheckPlayerConnectedToGameRequestDto data) : base() { }
    }
}
