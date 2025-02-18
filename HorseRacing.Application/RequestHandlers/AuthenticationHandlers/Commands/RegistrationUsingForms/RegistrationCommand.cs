using ErrorOr;
using HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Common;
using MediatR;

namespace HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Commands.RegistrationUsingForms
{
    public class RegistrationCommand : IRequest<ErrorOr<AuthenticationResult>>
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
        /// Адрес эл. почты
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        public RegistrationCommand(string userName, string password, string firstName, string lastName, string email, string phone)
        {
            UserName = userName;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
        }
    }
}
