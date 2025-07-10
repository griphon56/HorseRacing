using HorseRacing.Contracts.Base.Dto;
using HorseRacing.Contracts.Base.Responses;
using HorseRacing.Contracts.Models.Game.Dtos;

namespace HorseRacing.Contracts.Models.Game.Responses.GetAvailableSuit
{
    public class GetAvailableSuitResponse : BaseListResponse<GameAvailableSuitResponseDto>
    {
        public GetAvailableSuitResponse() { }
        public GetAvailableSuitResponse(List<GameAvailableSuitResponseDto> data) : base(data) { }
    }
}
