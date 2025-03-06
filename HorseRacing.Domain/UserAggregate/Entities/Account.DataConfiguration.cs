using HorseRacing.Domain.UserAggregate.ValueObjects;

namespace HorseRacing.Domain.UserAggregate.Entities
{
    public partial class Account
    {
        public static List<object> GetDefaultAccounts()
        {
            return new List<object>()
            {
                new {
                    Id = AccountId.Create(Account.DefaultAccountSystemAdministrator),
                    Balance = 0,
                    UserId = UserId.Create(User.DefaultSystemAdministrator)
                }
            };
        }

        /// <summary>
        /// Код аккаунта "Системный администратор"
        /// </summary>
        public static Guid DefaultAccountSystemAdministrator { get; } 
            = new Guid("1223998A-318F-460C-9464-1164EE56CB46");
    }
}
