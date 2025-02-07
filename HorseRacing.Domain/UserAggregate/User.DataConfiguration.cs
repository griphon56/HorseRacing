using HorseRacing.Domain.UserAggregate.ValueObjects;

namespace HorseRacing.Domain.UserAggregate
{
    public partial class User
    {
        /// <summary>
        /// Метод получения User по умолчанию
        /// </summary>
        public static List<object> GetDefaultUser()
        {
            return new List<object>()
            {
                new { Id = UserId.Create(User.DefaultSystemAdministrator),
                    UserName = "admin",
				    // Хэш пароля "7#v18eDq"
				    Password = "2A9BEAAD6666982A470776F098D3D9B6C41D1AB8D3286E7DC710DC6CE7393DB15BFDF9D0CE2C153E39A64D4FD9F04DC68FE5345BFCAA4ACB3F03",
                    FirstName = "Администратор",
                    LastName = "Системы",
                    Email = "admin@race.ru",
                    Phone = "79001112233",
                    DateCreated = new DateTime(2025,1,1).ToUniversalTime(),
                    IsRemoved = false
                }
            };

        }
        /// <summary>
        /// Код пользователя "Системный администратор"
        /// </summary>
        public static Guid DefaultSystemAdministrator { get; } = new Guid("8223998A-318F-460C-9464-1164EE56CB46");
    }
}
