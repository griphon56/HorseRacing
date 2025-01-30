using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace HorseRacing.Domain.UserAggregate
{
    /// <summary>
    /// Пользователь
    /// </summary>
    [Display(Description = "Пользователь")]
    public partial class User : AggregateRootChangeInfoGuid<UserId>
    {
        /// <summary>
        /// Конструктор без параметров <see cref="User"/>.
        /// </summary>
        private User() : base(UserId.CreateUnique(), new EntityChangeInfo(DateTime.UtcNow)) { }
        /// <summary>
        /// Логин
        /// </summary>
        public string UserName { get; private set; } = "";
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; private set; } = "";
        /// <summary>
        /// Признак "Удален"
        /// </summary>
        public bool IsRemoved { get; private set; }
        /// <summary>
		/// Имя
		/// </summary>
		public string FirstName { get; private set; } = "";
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; private set; } = "";
        /// <summary>
		/// Электронная почта
		/// </summary>
		public string Email { get; private set; } = "";
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; private set; } = "";
        /// <summary>
        /// Роль пользователя
        /// </summary>
        //public RoleId RoleId { get; private set; }
    }
}
