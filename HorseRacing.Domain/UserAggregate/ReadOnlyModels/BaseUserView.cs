using HorseRacing.Domain.UserAggregate.ValueObjects;

namespace HorseRacing.Domain.UserAggregate.ReadOnlyModels
{
    /// <summary>
	/// Базовая модель представления пользователя
	/// </summary>
	public class BaseUserView
    {
        /// <summary>
        /// Код пользователя
        /// </summary>
        public required UserId UserId { get; set; }
        /// <summary>
        ///  Логин
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
        /// Признак удаленного
        /// </summary>
        public bool IsRemoved { get; set; }
    }
}
