using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.Authentication.Requests.Registration
{
    public class RegistrationRequest : BaseRequest<RegistrationRequestDto>
    {
        public RegistrationRequest() : base() { }
        public RegistrationRequest(RegistrationRequestDto data) : base(data) { }
    }
}
