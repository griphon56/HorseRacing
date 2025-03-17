using HorseRacing.Domain.Common.Interfases;

namespace HorseRacing.Domain.GameAggregate.Events
{
    /// <summary>
	/// Игра окончена
	/// </summary>
	public record class GameOverEvent(Game game) : IDomainEvent;
}
