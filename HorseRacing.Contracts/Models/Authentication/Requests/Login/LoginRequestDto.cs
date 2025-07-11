using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Authentication.Requests.Login
{
    /// <summary>
    /// Модель запроса для авторизации
    /// </summary>
    public class LoginRequestDto : BaseDto
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
