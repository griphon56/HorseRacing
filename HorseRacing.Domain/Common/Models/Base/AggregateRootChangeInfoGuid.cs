using HorseRacing.Domain.UserAggregate.ValueObjects;

namespace HorseRacing.Domain.Common.Models.Base
{
    public abstract class AggregateRootChangeInfoGuid<TId> : AggregateRootGuid<TId>
          where TId : IdentityBaseGuid
    {
        private byte[] _versionRow;
        public byte[] VersionRow => _versionRow;

        /// <summary>
        /// Идентификатор сведений о дате создания.
        /// </summary>
        public DateTime DateCreated { get; private set; }
        /// <summary>
        /// Идентификатор сведений о созданном идентификаторе пользователя.
        /// </summary>
        public UserId? CreatedUserId { get; private set; }
        /// <summary>
        /// Идентификатор сведений о дате изменений.
        /// </summary>
        public DateTime? DateChanged { get; private set; }
        /// <summary>
        /// Идентификатор сведений о изменении идентификатора пользователя.
        /// </summary>
        public UserId? ChangedUserId { get; private set; }

        /// <summary>
        /// Конструктор без параметров <see cref="UserId"/> .
        /// </summary>
        protected AggregateRootChangeInfoGuid() : base() { }

        protected AggregateRootChangeInfoGuid(TId id, EntityChangeInfo ChangeInfo)
            : this(id, ChangeInfo?.DateCreated ?? DateTime.UtcNow, ChangeInfo?.CreatedUserId, ChangeInfo?.DateChanged, ChangeInfo?.ChangedUserId) { }

        /// <summary>
        /// Конструктор для создания на основе параметров <see cref="AggregateRootChangeInfoGuid"/> .
        /// </summary>
        /// <param name="dateCreated">Дата создания.</param>
        /// <param name="createdUserId">Идентификатор пользователя, создавшего запись.</param>
        /// <param name="dateChanged">Дата изменений.</param>
        /// <param name="changedUserId">Идентификаторы пользователя, изменившего запись.</param>
        protected AggregateRootChangeInfoGuid(TId id, DateTime dateCreated, UserId? createdUserId, DateTime? dateChanged = null, UserId? changedUserId = null) : base(id)
        {
            DateCreated = dateCreated;
            CreatedUserId = createdUserId;
            DateChanged = dateChanged;
            ChangedUserId = changedUserId;
        }
        /// <summary>
        /// Метод получения информации о изменении и добавлении агрегата. 
        /// </summary>
        public EntityChangeInfo GetEntityChangeInfo()
        {
            return new EntityChangeInfo(this.DateCreated, this.CreatedUserId, this.DateChanged, this.ChangedUserId);
        }

        public void SetDateChanged(DateTime? dateChanged)
        {
            DateChanged = dateChanged;
        }
        public void SetChangedUserId(UserId? changedUserId)
        {
            ChangedUserId = changedUserId;
        }
    }
}
