using HorseRacing.Domain.Common.Models.Specifications;

namespace HorseRacing.Domain.UserAggregate.Specifications
{
    public class GetActualUserSpecification : BaseSpecification<User>
    {
        /// <summary>
        /// Конструктор для создания на основе значения <see cref="UserName"/> .
        /// </summary>
        public GetActualUserSpecification() : base(u => !u.IsRemoved) { }
    }
}
