using HorseRacing.Contracts.Base.Responses;

namespace HorseRacing.Contracts.Models.Authentication.Responses.Authentication
{
    /// <summary>
    /// Модель ответа при авторизации
    /// </summary>
    public class AuthenticationResponse : BaseResponse<AuthenticationResponseDto>
    {
        public AuthenticationResponse() : base() { }

        public AuthenticationResponse(AuthenticationResponseDto data) : base(data) { }
    }
}
