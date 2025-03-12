﻿using HorseRacing.Domain.UserAggregate.ValueObjects;
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
                }
            };
        }

        /// <summary>
        /// Код пользователя "Системный администратор"
        /// </summary>
        public static Guid DefaultSystemAdministrator { get; } = new Guid("8223998A-318F-460C-9464-1164EE56CB46");
    }
}
