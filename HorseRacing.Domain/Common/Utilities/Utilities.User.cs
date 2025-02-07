namespace HorseRacing.Domain.Common.Utilities
{
    /// <summary>
	/// Утилиты для работы с пользователем
	/// </summary>
	public static partial class Utilities
    {
        public static class User
        {
            /// <summary>
            /// Метод форматирования информации пользователя в Фамилия Имя
            /// </summary>
            /// <param name="fname">Имя</param>
            /// <param name="lname">Фамилия</param>
            public static string FormatLFM(string fname, string lname)
            {
                string result = string.Empty;

                if (!string.IsNullOrEmpty(fname))
                    result += $"{fname.Trim()} ";
                if (!string.IsNullOrEmpty(lname))
                    result += $"{lname.Trim()} ";

                return result.Trim();
            }
            /// <summary>
            /// Метод форматирования информации пользователя в Фамилия Имя
            /// </summary>
            /// <param name="user">Пользователь</param>
            public static string FormatLFM(UserAggregate.User user)
            {
                return FormatLFM(user.FirstName, user.LastName);
            }
            /// <summary>
            /// Метод форматирования информации пользователя в Имя Фамилия
            /// </summary>
            /// <param name="fname">Имя</param>
            /// <param name="lname">Фамилия</param>
            /// <param name="mname">Отчество</param>
            public static string FormatFML(string fname, string lname)
            {
                string result = string.Empty;

                if (!string.IsNullOrEmpty(fname))
                    result += $"{fname.Trim()} ";
                if (!string.IsNullOrEmpty(lname))
                    result += $"{lname.Trim()}";

                return result.Trim();
            }
            /// <summary>
            /// Метод форматирования информации пользователя в Фамилия Имя (username)
            /// </summary>
            /// <param name="fname">Имя</param>
            /// <param name="lname">Фамилия</param>
            /// <param name="uname">Логин</param>
            public static string FormatLFMUsername(string fname, string lname, string uname = "")
            {
                string result = string.Empty;

                if (!string.IsNullOrEmpty(fname))
                    result += $"{fname.Trim()} ";
                if (!string.IsNullOrEmpty(lname))
                    result += $"{lname.Trim()} ";
                if (!string.IsNullOrEmpty(uname))
                    result += $"({uname.Trim()})";

                return result.Trim();
            }
        }
    }
}
