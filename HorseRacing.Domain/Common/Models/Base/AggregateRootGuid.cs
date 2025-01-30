using HorseRacing.Domain.Common.Interfases;

namespace HorseRacing.Domain.Common.Models.Base
{
    /// <summary>
	/// Абстрактный класс корня агрегата
	/// </summary>
	/// <typeparam name="TId"></typeparam>
	/// <typeparam name="TIdType"></typeparam>
	public abstract class AggregateRootGuid<TId> : EntityGuid<TId>, IAggregateRoot<TId>
        where TId : IdentityBaseGuid

    {
        protected AggregateRootGuid() : base() { }

        protected AggregateRootGuid(TId id) : base(id) { }

        public virtual Func<List<string>> GetIgnoreProperties(List<string> ignoreProps = null)
        {
            var result = new List<string> { nameof(this.DomainEvents) };
            if (ignoreProps != null)
            {
                result.AddRange(ignoreProps);
            }

            return () => result;
        }
    }
}
