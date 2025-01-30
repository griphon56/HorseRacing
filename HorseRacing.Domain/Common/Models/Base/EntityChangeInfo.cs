using HorseRacing.Domain.UserAggregate.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace HorseRacing.Domain.Common.Models.Base
{
    public class EntityChangeInfo
    {
        public EntityChangeInfo() { }

        /// <summary>
        /// Конструктор для создания на основе параметров <see cref="EntityChangeInfo"/> .
        /// </summary>
        /// <param name="dateCreated">Дата создания</param>
        /// <param name="createdUserId">Код пользователя, который выполнил создание</param>
        /// <param name="dateChanged">Дата изменения</param>
        /// <param name="changedUserId">Код пользователя, который внес изменения</param>
        [SetsRequiredMembers]
        public EntityChangeInfo(DateTime dateCreated, UserId? createdUserId = null, DateTime? dateChanged = null, UserId? changedUserId = null)
        {
            DateCreated = dateCreated;
            CreatedUserId = createdUserId;
            DateChanged = dateChanged;
            ChangedUserId = changedUserId;
        }
        [SetsRequiredMembers]
        public EntityChangeInfo(EntityChangeInfo entityChangeInfo, DateTime? dateChanged = null, UserId? changedUserId = null)
        {
            DateCreated = entityChangeInfo.DateCreated;
            CreatedUserId = entityChangeInfo.CreatedUserId;
            DateChanged = dateChanged;
            ChangedUserId = changedUserId;
        }

        /// <summary>
        /// Дата создания
        /// </summary>
        public required DateTime DateCreated { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// Код пользователя, который выполнил создание
        /// </summary>
        public UserId? CreatedUserId { get; set; }
        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime? DateChanged { get; set; }
        /// <summary>
        /// Код пользователя, который внес изменения
        /// </summary>
        public UserId? ChangedUserId { get; set; }
    }
}
