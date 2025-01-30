namespace HorseRacing.Domain.Common.Interfases
{
    /// <summary>
	/// Интерфейс события домена
	/// </summary>
	public interface IHasDomainEvents
    {
        /// <summary>
        /// Список событий домена
        /// </summary>
        public IReadOnlyList<IDomainEvent> DomainEvents { get; }
        /// <summary>
        /// Очистить события домена
        /// </summary>
        public void ClearDomainEvents();
    }
}
