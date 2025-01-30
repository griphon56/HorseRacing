namespace HorseRacing.Domain.Common.Interfases
{
    /// <summary>
	/// Интерфейс корня агрегата
	/// </summary>
	/// <typeparam name="TId"></typeparam>
	public interface IAggregateRoot<TId> : IEntity<TId>
        where TId : notnull
    {
    }
}
