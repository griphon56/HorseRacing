using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Authentication.Dtos
{
    public class RegistrationRequestDto : BaseDto
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; } = string.Empty;
    }
}
