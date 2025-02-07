using HorseRacing.Domain.Common.Interfases;
using System.Linq.Expressions;

namespace HorseRacing.Domain.Common.Models.Specifications
{
    /// <summary>
	/// Базовая спецификация.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class BaseSpecification<T> : ISpecification<T>
    {
        /// <summary>
        /// Конструктор класса BaseSpecification с заданным предикатом для использования в контексте этого класса или связанных с ним операциях.
        /// </summary>
        public BaseSpecification(Expression<Func<T, bool>> expression)
        {
            Expression = expression;
        }
        /// <summary>
        /// Метод использует предикатное выражение, хранящееся в свойстве Expression, для проверки соответствия объекта с определенными критериями.
        /// </summary>
        public virtual bool IsSatisfiedBy(T obj)
        {
            return Expression.Compile()(obj);
        }
        /// <summary>
        /// Свойство Expression имеет тип Expression<Func<T, bool>>
        /// </summary>
        public Expression<Func<T, bool>> Expression { get; private set; }
        /// <summary>
        /// Определяет неявный оператор преобразования из типа BaseSpecification<T> в тип Expression<Func<T, bool>>.
        /// </summary>
        /// <param name="specification"></param>
        public static implicit operator Expression<Func<T, bool>>(BaseSpecification<T> specification)
        {
            return specification.Expression;
        }
    }
}
