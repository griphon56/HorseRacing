using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.Authentication.Responses.Authentication
{
    public class AuthenticationResponseDto : BaseDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// Почтовый ящик
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Токен авторизации
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
