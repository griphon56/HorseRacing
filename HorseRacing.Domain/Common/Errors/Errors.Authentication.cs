using ErrorOr;

namespace HorseRacing.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Authentication
        {
            /// <summary>
            /// Ошибка, возникающая, когда пользователь с указанным логином не найден. 
            /// </summary>
            public static Error UserNotFound => Error.Validation($"{nameof(UserAggregate.User)}.{nameof(UserNotFound)}", "Пользователь не найден");
            /// <summary>
            /// Ошибка, возникающая, когда пароль указан неверно. 
            /// </summary>
            public static Error PasswordIncorrect => Error.Validation($"{nameof(UserAggregate.User)}.{nameof(PasswordIncorrect)}", "Пароль указан неверно");
        }
    }
}
