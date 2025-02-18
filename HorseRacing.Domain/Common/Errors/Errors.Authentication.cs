using ErrorOr;

namespace HorseRacing.Domain.Common.Errors
{
    public static partial class Errors
    {
        /// <summary>
        /// Объявление статистических классов аутентификации.
        /// </summary>
        public static class Authentication
        {
            /// <summary>
            /// Ошибка, возникающая, когда пользователь с указанным логином не найден. 
            /// </summary>
            public static Error NotFoundUser => Error.Validation($"{nameof(UserAggregate.User)}.{nameof(NotFoundUser)}", "Пользователь с указанным логином не найден");
            /// <summary>
            /// Ошибка, возникающая, когда пароль указан неверно. 
            /// </summary>
            public static Error PasswordIncorrect => Error.Validation($"{nameof(UserAggregate.User)}.{nameof(PasswordIncorrect)}", "Пароль указан неверно");
        }
    }
}
