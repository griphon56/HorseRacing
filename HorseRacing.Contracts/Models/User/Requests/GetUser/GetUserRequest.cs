using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.User.Requests.GetUser
{
    public class GetUserRequest : BaseRequest<GetUserRequestDto>
    {
        public GetUserRequest() : base() { }
        public GetUserRequest(GetUserRequestDto data) : base(data) { }
    }
}
