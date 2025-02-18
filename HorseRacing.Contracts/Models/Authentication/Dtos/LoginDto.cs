using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Authentication.Dtos
{
    /// <summary>
    /// Модель запроса для авторизации
    /// </summary>
    public class LoginDto : BaseDto
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
