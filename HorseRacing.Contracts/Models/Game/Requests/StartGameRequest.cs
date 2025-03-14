using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.Game.Requests
{
    public class StartGameRequest : BaseRequest<BaseModelDto>
    {
        public StartGameRequest() : base() { }
        public StartGameRequest(BaseModelDto data) : base(data) { }
    }
}
