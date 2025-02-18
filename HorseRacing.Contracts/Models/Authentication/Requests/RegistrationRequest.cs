using HorseRacing.Contracts.Base.Requests;
using HorseRacing.Contracts.Models.Authentication.Dtos;

namespace HorseRacing.Contracts.Models.Authentication.Requests
{
    public class RegistrationRequest : BaseRequest<RegistrationRequestDto>
    {
        public RegistrationRequest() : base() { }
        public RegistrationRequest(RegistrationRequestDto data) : base(data) { }
    }
}
