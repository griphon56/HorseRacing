using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.User.Requests.GetUser
{
    public class GetUserRequestDto : BaseDto
    {
        public Guid UserId { get; set; }
    }
}
