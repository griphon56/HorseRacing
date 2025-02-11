using HorseRacing.Application.Base;
using HorseRacing.Domain.UserAggregate;

namespace HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Common
{
    /// <summary>
    /// Результат аутентификации.
    /// </summary>
    public class AuthenticationResult : BaseModelResult
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public required User User { get; set; }
        /// <summary>
        ///  Токен
        /// </summary>
        public required string Token { get; set; } = string.Empty;
    }
}
