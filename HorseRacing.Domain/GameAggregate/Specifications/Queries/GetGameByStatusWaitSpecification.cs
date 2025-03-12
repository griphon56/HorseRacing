using HorseRacing.Domain.Common.Models.Specifications;

namespace HorseRacing.Domain.GameAggregate.Specifications.Queries
{
    public class GetGameByStatusWaitSpecification : BaseSpecification<Game>
    {
        public GetGameByStatusWaitSpecification() : base(x => x.Status == Enums.StatusType.Wait) { }
    }
}
