using HorseRacing.Contracts.Base.Requests;
using HorseRacing.Contracts.Models.Authentication.Dtos;

namespace HorseRacing.Contracts.Models.Authentication.Requests
{
    /// <summary>
    /// Модель запроса для авторизации
    /// </summary>
    public class LoginRequest : BaseRequest<LoginDto>
    {
        public LoginRequest() : base() { }
        public LoginRequest(LoginDto data) : base(data) { }
    }
}
