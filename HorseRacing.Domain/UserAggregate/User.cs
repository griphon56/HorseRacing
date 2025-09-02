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
        public string UserName { get; private set; } = string.Empty;
        /// <summary>
        /// Хеш пароля
        /// </summary>
        public string HashPassword { get; private set; } = string.Empty;
        /// <summary>
        /// Пароль
        /// </summary>
        public byte[] Password { get; private set; }
        /// <summary>
        /// Признак "Удален"
        /// </summary>
        public bool IsRemoved { get; private set; }
        /// <summary>
		/// Имя
		/// </summary>
		public string FirstName { get; private set; } = string.Empty;
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; private set; } = string.Empty;
        /// <summary>
		/// Электронная почта
		/// </summary>
		public string Email { get; private set; } = string.Empty;
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; private set; } = string.Empty;

        private Account? _account;
        public Account? Account => _account;

        /// <summary>
        /// Роль пользователя
        /// </summary>
        //public RoleId RoleId { get; private set; }

        private User() : base(UserId.CreateUnique(), new EntityChangeInfo(DateTime.UtcNow)) { }

        private User(UserId id, string userName, string hashPassword, byte[] password, string firstName, string lastName
            , string email, string phone, EntityChangeInfo changeInfo, bool isRemoved)
            : base(id ?? UserId.CreateUnique(), changeInfo)
        {
            UserName = userName.Trim();
            HashPassword = hashPassword.Trim();
            Password = password;
            IsRemoved = isRemoved;
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim();
            Phone = phone.Trim();
        }

        public static User Create(UserId id, string userName, string hashPassword, byte[] password, string firstName
            , string lastName, string email, string phone, EntityChangeInfo changeInfo, bool isRemoved = false)
        {
            var user = new User(id, userName, hashPassword, password, firstName, lastName, email, phone, changeInfo, isRemoved);

            user._account = Account.Create(AccountId.CreateUnique(), 100, id);

            return user;
        }

        public void Update(string firstName, string lastName, string email, string phone, EntityChangeInfo changeInfo)
        {
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim();
            Phone = phone.Trim();

            SetDateChanged(changeInfo.DateChanged);
            SetChangedUserId(changeInfo.ChangedUserId);
        }

        public void UpdatePassword(string? hashPassword, byte[]? password)
        {
            if (!string.IsNullOrEmpty(hashPassword) && password is not null)
            {
                HashPassword = hashPassword.Trim();
                Password = password;
            }
        }
    }
}
