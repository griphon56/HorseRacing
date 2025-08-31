using HorseRacing.Contracts.Base.Responses;

namespace HorseRacing.Contracts.Models.User.Responses.GetUser
{
    public class GetUserResponse : BaseResponse<GetUserResponseDto>
    {
        public GetUserResponse() : base() { }
        public GetUserResponse(GetUserResponseDto data) : base(data) { }
    }
}
