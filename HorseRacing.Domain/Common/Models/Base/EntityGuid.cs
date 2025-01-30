using HorseRacing.Domain.Common.Interfases;

namespace HorseRacing.Domain.Common.Models.Base
{
	public abstract class EntityGuid<TId> : IEquatable<TId>, IEntity<TId>, IHasDomainEvents
        where TId : IdentityBaseGuid
    {
        /// <summary>
        /// Список событий домена только для просмотра.
        /// </summary>
        private readonly List<IDomainEvent> _domainEvents = new();
        /// <summary>
        /// События домена.
        /// </summary>
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        /// <summary>
        /// Идентификатор
        /// </summary>
        public TId Id { get; }

#pragma warning disable CS8618
        protected EntityGuid() { }

        protected EntityGuid(TId id)
        {
            Id = id;
        }
#pragma warning restore CS8618

        /// <summary>
        /// Позволяет сравнивать объекты этого класса на основе их Id.
        /// </summary>
        /// <param name="obj">Объект.</param>
        public override bool Equals(object? obj)
        {
            if (obj is null && Id is null)
            {
                return true;
            }
            if (obj is not null && Id is null)
            {
                return false;
            }
            return obj is EntityGuid<TId> entity && Id.Equals(entity.Id);
        }

        /// <summary>
        /// Метод выполняет сравнение текущего объекта с объектом, переданным в параметре other, используя метод Equals(object?).
        /// </summary>
        /// <param name="other"></param>
        public bool Equals(TId? other)
        {
            return Equals((object?)other);
        }

        public static bool operator ==(EntityGuid<TId> left, EntityGuid<TId> right)
        {
            return Equals(left, right);
        }
        public static bool operator !=(EntityGuid<TId> left, EntityGuid<TId> right)
        {
            return !Equals(left, right);
        }
        /// <summary>
        /// Вычисление хеш-кода объекта
        /// </summary>
        public override int GetHashCode()
        {
            if (Id is null)
            {
                return 0;
            }
            return Id.GetHashCode();
        }

        /// <summary>
        /// Метод AddDomainEvent позволяет добавить событие в коллекцию _domainEvents, чтобы отслеживать и управлять доменными событиями в системе.
        /// </summary>
        /// <param name="domainEvent">Событие домена.</param>
        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        /// <summary>
        /// Очистить события домена.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
