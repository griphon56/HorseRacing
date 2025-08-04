using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.Game.Requests.PlayGame
{
    public class PlayGameRequest : BaseRequest<BaseModelDto>
    {
        public PlayGameRequest() : base() { }
        public PlayGameRequest(BaseModelDto data) : base(data) { }
    }
}
