using HorseRacing.Domain.Common.Interfases;

namespace HorseRacing.Domain.Common.Models.Base
{
    /// <summary>
	/// Корень агрегата целочисленного значения.
	/// </summary>
	public abstract class AggregateRootInt<TId> : EntityInt<TId>, IAggregateRoot<TId>
        where TId : IdentityBaseInt

    {
        protected AggregateRootInt(TId id) : base(id) { }
    }
}
