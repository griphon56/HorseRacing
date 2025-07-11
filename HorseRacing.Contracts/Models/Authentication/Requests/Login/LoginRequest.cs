using HorseRacing.Contracts.Base.Requests;

namespace HorseRacing.Contracts.Models.Authentication.Requests.Login
{
    /// <summary>
    /// Модель запроса для авторизации
    /// </summary>
    public class LoginRequest : BaseRequest<LoginRequestDto>
    {
        public LoginRequest() : base() { }
        public LoginRequest(LoginRequestDto data) : base(data) { }
    }
}
