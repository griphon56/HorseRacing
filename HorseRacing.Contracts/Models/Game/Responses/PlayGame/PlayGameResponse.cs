using HorseRacing.Contracts.Base.Responses;

namespace HorseRacing.Contracts.Models.Game.Responses.PlayGame
{
    public class PlayGameResponse : BaseResponse<PlayGameResponseDto>
    {
        public PlayGameResponse() : base() { }
        public PlayGameResponse(PlayGameResponseDto data) : base(data) { }
    }
}
