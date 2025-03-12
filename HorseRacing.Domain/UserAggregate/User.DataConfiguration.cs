using HorseRacing.Domain.UserAggregate.ValueObjects;
using System;

namespace HorseRacing.Domain.UserAggregate
{
    public partial class User
    {
        public static List<object> GetDefaultUsers()
        {
            return new List<object>()
            {
                new {
                    Id = UserId.Create(User.DefaultSystemAdministrator),
                    UserName = "admin",
				    // Хэш пароля "qwerty_123"
				    Password = "82E3B4B3D57F6D4112A02310EA2E8F9517BC19BD6EBF1EC95E0C7AA961B3B3F2AE24BB2CA278CB190D24B55241AC893C9E590717106F3FB18070",
                    FirstName = "Администратор",
                    LastName = "Системы",
                    Email = "admin@race.ru",
                    Phone = "79001112233",
                    DateCreated = new DateTime(2025,1,1).ToUniversalTime(),
                    IsRemoved = false,
                    _versionRow = new byte[] { 0x01 }
                },
                new {
                    Id = UserId.Create(new Guid("8F2FACBC-2EF4-4FE1-B9E0-E3F877EDB3C3")),
                    UserName = "ivan",
				    // Хэш пароля "1"
				    Password = "A44DC50E7693F034C1C6032F04E7E5152729DF8891D15DD1F485F7C98FE62AA0113FCB77745A024BF3573AD54837D6D8D0DDD61E27E06B1DC18F",
                    FirstName = "Иван",
                    LastName = "Пика",
                    Email = "ivan@race.ru",
                    Phone = "79001112211",
                    DateCreated = new DateTime(2025,1,1).ToUniversalTime(),
                    IsRemoved = false,
                    _versionRow = new byte[] { 0x01 }
                },
                new {
                    Id = UserId.Create(new Guid("141FCB82-6639-4932-A68D-AF84F09EF42D")),
                    UserName = "petr",
				    // Хэш пароля "1"
				    Password = "A44DC50E7693F034C1C6032F04E7E5152729DF8891D15DD1F485F7C98FE62AA0113FCB77745A024BF3573AD54837D6D8D0DDD61E27E06B1DC18F",
                    FirstName = "Петр",
                    LastName = "Крести",
                    Email = "petr@race.ru",
                    Phone = "79001112222",
                    DateCreated = new DateTime(2025,1,1).ToUniversalTime(),
                    IsRemoved = false,
                    _versionRow = new byte[] { 0x01 }
                },
                new {
                    Id = UserId.Create(new Guid("A1F52F7E-6DD6-4FF9-BDED-78305B42C81E")),
                    UserName = "john",
				    // Хэш пароля "1"
				    Password = "A44DC50E7693F034C1C6032F04E7E5152729DF8891D15DD1F485F7C98FE62AA0113FCB77745A024BF3573AD54837D6D8D0DDD61E27E06B1DC18F",
                    FirstName = "Джонни",
                    LastName = "Черва",
                    Email = "john@race.ru",
                    Phone = "79001112233",
                    DateCreated = new DateTime(2025,1,1).ToUniversalTime(),
                    IsRemoved = false,
                    _versionRow = new byte[] { 0x01 }
                },
                new {
                    Id = UserId.Create(new Guid("17B6F19E-56B1-485C-A16A-C60CEC5CDAA6")),
                    UserName = "alex",
				    // Хэш пароля "1"
				    Password = "A44DC50E7693F034C1C6032F04E7E5152729DF8891D15DD1F485F7C98FE62AA0113FCB77745A024BF3573AD54837D6D8D0DDD61E27E06B1DC18F",
                    FirstName = "Алексей",
                    LastName = "Бубна",
                    Email = "alex@race.ru",
                    Phone = "79001112244",
                    DateCreated = new DateTime(2025,1,1).ToUniversalTime(),
                    IsRemoved = false,
                    _versionRow = new byte[] { 0x01 }
                }
            };
        }

        /// <summary>
        /// Код пользователя "Системный администратор"
        /// </summary>
        public static Guid DefaultSystemAdministrator { get; } = new Guid("8223998A-318F-460C-9464-1164EE56CB46");
    }
}
