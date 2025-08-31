using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Models.User.Responses.GetUser
{
    public class GetUserResponseDto : BaseDto
    {
        /// <summary>
		/// Имя
		/// </summary>
		public string FirstName { get; set; } = "";
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; } = "";
        /// <summary>
        /// Фамилия
        /// </summary>
        public string UserName { get; set; } = "";
        /// <summary>
		/// Электронная почта
		/// </summary>
		public string Email { get; set; } = "";
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; } = "";
    }
}
