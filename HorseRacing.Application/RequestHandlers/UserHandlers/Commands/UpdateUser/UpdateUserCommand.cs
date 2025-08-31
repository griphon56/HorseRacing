using ErrorOr;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.UserHandlers.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<ErrorOr<Unit>>
    {
        /// <summary>
        /// Код пользователя
        /// </summary>
        public UserId UserId { get; set; }
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
        public string Password { get; set; } = string.Empty;
    }
}
