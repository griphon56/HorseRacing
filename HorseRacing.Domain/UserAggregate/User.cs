using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Domain.UserAggregate.Entities;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace HorseRacing.Domain.UserAggregate
{
    /// <summary>
    /// Агрегат "Пользователь"
    /// </summary>
    [Display(Description = "Пользователь")]
    public partial class User : AggregateRootChangeInfoGuid<UserId>
    {
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

        private readonly Account _account;
        public Account Account => _account;

        /// <summary>
        /// Роль пользователя
        /// </summary>
        //public RoleId RoleId { get; private set; }

        private User() : base(UserId.CreateUnique(), new EntityChangeInfo(DateTime.UtcNow)) { }

        private User(UserId id, string userName, string password, string firstName, string lastName
            , string email, string phone, EntityChangeInfo changeInfo, bool isRemoved, Account? account) 
            : base(id ?? UserId.CreateUnique(), changeInfo)
        {
            UserName = userName;
            Password = password;
            IsRemoved = isRemoved;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            _account = account;
        }

        public static User Create(UserId id, string userName, string password, string firstName
            , string lastName, string email, string phone, EntityChangeInfo changeInfo, bool isRemoved = true)
        {
            Account account = Account.Create(AccountId.CreateUnique(), 0, id);

            var user = new User(id, userName, password, firstName, lastName, email, phone, changeInfo, isRemoved, account);

            return user;
        }
    }
}
