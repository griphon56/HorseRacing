using HorseRacing.Domain.Common.Models.Specifications;

namespace HorseRacing.Domain.UserAggregate.Specifications
{
    /// <summary>
	/// Поиск по спецификации имени пользователя.
	/// </summary>
	public class GetUserByUserNameSpecification : BaseSpecification<User>
    {
        /// <summary>
        /// Конструктор для создания на основе значения <see cref="UserName"/> .
        /// </summary>
        public GetUserByUserNameSpecification(string UserName) : base(user => user.UserName == UserName) { }
    }
}
