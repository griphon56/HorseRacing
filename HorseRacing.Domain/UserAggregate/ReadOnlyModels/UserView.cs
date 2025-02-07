using HorseRacing.Domain.Common.Utilities;

namespace HorseRacing.Domain.UserAggregate.ReadOnlyModels
{
    public class UserView : BaseUserView
    {
        /// <summary>
		/// Полное наименование пользователя ФИО
		/// </summary>
		public string FullName { get { return Utilities.User.FormatLFM(this.FirstName, this.LastName); } }
        /// <summary>
        /// Полное наименование пользователя ФИО (логин)
        /// </summary>
        public string FullAccountName { get { return Utilities.User.FormatLFMUsername(this.FirstName, this.LastName, this.UserName); } }
        /// <summary>
        /// Электронная почта
        /// </summary>
        public required string Email { get; set; }
        /// <summary>
        /// Телефон
        /// </summary>
        public required string Phone { get; set; }
    }
}
