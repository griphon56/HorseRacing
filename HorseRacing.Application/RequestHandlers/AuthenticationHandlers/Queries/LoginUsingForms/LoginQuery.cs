using ErrorOr;
using HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Common;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Queries.LoginUsingForms
{
    /// <summary>
    /// Кодманда авторизации
    /// </summary>
    public class LoginQuery : IRequest<ErrorOr<AuthenticationResult>>
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
