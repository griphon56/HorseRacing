namespace HorseRacing.Domain.Common.Interfases
{
    /// <summary>
	/// Интерфейс сущности.
	/// </summary>
	/// <typeparam name="TId"></typeparam>
	public interface IEntity<TId>
        where TId : notnull
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        /// <value>Значение TId.</value>
        public TId Id { get; }
    }
}
