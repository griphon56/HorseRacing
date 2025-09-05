using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.User.Requests.UpdateUser
{
    public class UpdateUserRequestDto : BaseDto
    {
        /// <summary>
        /// Код пользователя
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
		/// Имя
		/// </summary>
		public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// Фамилия
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
		/// Электронная почта
		/// </summary>
		public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; } = string.Empty;
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }
    }
}
