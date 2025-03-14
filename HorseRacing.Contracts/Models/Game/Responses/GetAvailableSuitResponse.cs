using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Contracts.Base.Responses;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Responses
{
    public class GetAvailableSuitResponse : BaseListResponse<GameAvailableSuitDto>
    {
        public GetAvailableSuitResponse() { }
        public GetAvailableSuitResponse(List<GameAvailableSuitDto> data) : base(data) { }
    }
}
