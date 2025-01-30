using System.Linq.Expressions;

namespace HorseRacing.Domain.Common.Interfases
{
    /// <summary>
	/// Интерфейс спецификации
	/// </summary>
	public interface ISpecification<T>
    {
        /// <summary>
        /// <T, obj> сигнатура которых совпадает с IsSatisfiedBy
        /// </summary>
        public bool IsSatisfiedBy(T obj);
        /// <summary>
        /// Выражение
        /// </summary>
        Expression<Func<T, bool>> Expression { get; }
    }
}
