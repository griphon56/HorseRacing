using HorseRacing.Contracts.Base.Responses;
using HorseRacing.Contracts.Models.Authentication.Dtos;

namespace HorseRacing.Contracts.Models.Authentication.Responses
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
