using HorseRacing.Contracts.Base.Responses;

namespace HorseRacing.Contracts.Models.Game.Responses.CheckPlayerConnectedToGame
{
    public class CheckPlayerConnectedToGameResponse : BaseResponse<CheckPlayerConnectedToGameResponseDto>
    {
        public CheckPlayerConnectedToGameResponse() : base() { }
        public CheckPlayerConnectedToGameResponse(CheckPlayerConnectedToGameResponseDto data) : base(data) { }
    }
}
