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
                },
                new {
                    Id = AccountId.Create(new Guid("6E61DBEC-D4C9-4F17-BA0D-2920D714E3D8")),
                    Balance = 10,
                    UserId = UserId.Create(User.TestUserIvan)
                },
                new {
                    Id = AccountId.Create(new Guid("12E3693B-C52C-418B-8053-7F1D2C173F73")),
                    Balance = 10,
                    UserId = UserId.Create(User.TestUserPetr)
                },
                new {
                    Id = AccountId.Create(new Guid("B43AB810-B393-454D-B35F-15975D20E33B")),
                    Balance = 10,
                    UserId = UserId.Create(User.TestUserJohn)
                },
                new {
                    Id = AccountId.Create(new Guid("7435E47C-7336-4C28-BADC-0FA526756616")),
                    Balance = 10,
                    UserId = UserId.Create(User.TestUserAlex)
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
