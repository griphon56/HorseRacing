using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.Game.Requests.PlaceBet
{
    public class PlaceBetRequest : BaseRequest<PlaceBetRequestDto>
    {
        public PlaceBetRequest() : base() { }
        public PlaceBetRequest(PlaceBetRequestDto data) : base(data) { }
    }
}
