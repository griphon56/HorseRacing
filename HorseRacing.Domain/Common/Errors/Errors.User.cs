using ErrorOr;

namespace HorseRacing.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class User
        {
            /// <summary>
            /// Пользователь с указанным логином уже существует 
            /// </summary>
            public static Error DuplicateUserName => Error.Conflict($"{nameof(UserAggregate.User)}.{nameof(DuplicateUserName)}", "Пользователь с указанным логином уже существует");
            /// <summary>
            /// Пользователь с указанным почтовым ящиком уже существует 
            /// </summary>
            public static Error DuplicateEmail => Error.Conflict($"{nameof(UserAggregate.User)}.{nameof(DuplicateEmail)}", "Пользователь с указанным почтовым ящиком уже существует");
            /// <summary>
            /// Не определен Пользователь, работающий в данном контексте
            /// </summary>
            public static Error WorkingUserNotFound => Error.Failure($"{nameof(UserAggregate.User)}.{nameof(WorkingUserNotFound)}", "Не определен Пользователь, работающий в данном контексте");
            /// <summary>
            /// Пользователь не найден в БД.
            /// </summary>
            public static Error UserNotFound => Error.Failure($"{nameof(UserAggregate.User)}.{nameof(UserNotFound)}", "Пользователь не найден");
            /// <summary>
            /// Не заполнено поле: "Имя"
            /// </summary>
            public static Error FirstNameNotFilled => Error.Validation($"{nameof(UserAggregate.User)}.{nameof(FirstNameNotFilled)}", "Не заполнено поле: \"Имя\"");
            /// <summary>
            /// Не заполнено поле: "Фамилия"
            /// </summary>
            public static Error LastNameNotFilled => Error.Validation($"{nameof(UserAggregate.User)}.{nameof(LastNameNotFilled)}", "Не заполнено поле: \"Фамилия\"");
        }
    }
}
