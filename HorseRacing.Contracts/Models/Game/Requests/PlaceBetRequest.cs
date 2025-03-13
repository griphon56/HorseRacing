using HorseRacing.Contracts.Base.Requests;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Requests
{
    public class PlaceBetRequest : BaseRequest<PlaceBetRequestDto>
    {
        public PlaceBetRequest() : base() { }
        public PlaceBetRequest(PlaceBetRequestDto data) : base(data) { }
    }
}
