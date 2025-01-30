using HorseRacing.Domain.UserAggregate.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace HorseRacing.Domain.Common.Models.Base
{
    public class EntityCreateInfo
    {
        public EntityCreateInfo() { }

        /// <summary>
        /// Конструктор для создания на основе параметров <see cref="EntityCreateInfo"/> .
        /// </summary>
        /// <param name="dateCreated">Дата создания</param>
        /// <param name="createdUserId">Код пользователя, который выполнил создание</param>
        [SetsRequiredMembers]
        public EntityCreateInfo(DateTime dateCreated, UserId? createdUserId = null)
        {
            DateCreated = dateCreated;
            CreatedUserId = createdUserId;
        }
        /// <summary>
        /// Дата создания
        /// </summary>
        public required DateTime DateCreated { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// Код пользователя, который выполнил создание
        /// </summary>
        public UserId? CreatedUserId { get; set; }
    }
}
