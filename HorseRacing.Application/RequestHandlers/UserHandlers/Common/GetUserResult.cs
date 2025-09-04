using HorseRacing.Application.Base;

namespace HorseRacing.Application.RequestHandlers.UserHandlers.Common
{
    public class GetUserResult : BaseResult
    {
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
    }
}
