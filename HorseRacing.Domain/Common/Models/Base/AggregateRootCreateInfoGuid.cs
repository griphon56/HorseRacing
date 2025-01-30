using HorseRacing.Domain.UserAggregate.ValueObjects;

namespace HorseRacing.Domain.Common.Models.Base
{
    public abstract class AggregateRootCreateInfoGuid<TId> : AggregateRootGuid<TId>
          where TId : IdentityBaseGuid
    {
        /// <summary>
        /// Конструктор без параметров <see cref="UserId"/> .
        /// </summary>
        protected AggregateRootCreateInfoGuid() : base() { }

        protected AggregateRootCreateInfoGuid(TId id, EntityCreateInfo CreateInfo) :
            this(id, CreateInfo?.DateCreated ?? DateTime.UtcNow, CreateInfo?.CreatedUserId)
        { }

        /// <summary>
        /// Конструктор для создания на основе параметров <see cref="AggregateRootCreateInfoGuid"/> .
        /// </summary>
        /// <param name="dateCreated">Дата создания.</param>
        /// <param name="createdUserId">Идентификатор пользователя, создавшего запись.</param>
        protected AggregateRootCreateInfoGuid(TId id, DateTime dateCreated, UserId? createdUserId) : base(id)
        {
            DateCreated = dateCreated;
            CreatedUserId = createdUserId;
        }

        /// <summary>
        /// Идентификатор сведений о дате создания.
        /// </summary>
        public DateTime DateCreated { get; private set; }
        /// <summary>
        /// Идентификатор сведений о созданном идентификаторе пользователя.
        /// </summary>
        public UserId? CreatedUserId { get; private set; }
    }
}
