using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.User.Requests.UpdateUser
{
    public class UpdateUserRequest : BaseRequest<UpdateUserRequestDto>
    {
        public UpdateUserRequest() : base() { }
        public UpdateUserRequest(UpdateUserRequestDto data) : base(data) { }
    }
}
