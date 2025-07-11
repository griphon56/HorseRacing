using HorseRacing.Contracts.Base.Responses;

namespace HorseRacing.Contracts.Models.Game.Responses.GetAvailableSuit
{
    public class GetAvailableSuitResponse : BaseListResponse<GetAvailableSuitResponseDto>
    {
        public GetAvailableSuitResponse() { }
        public GetAvailableSuitResponse(List<GetAvailableSuitResponseDto> data) : base(data) { }
    }
}
