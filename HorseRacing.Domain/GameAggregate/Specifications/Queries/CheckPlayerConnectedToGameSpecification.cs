using HorseRacing.Domain.Common.Models.Specifications;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate.ValueObjects;

namespace HorseRacing.Domain.GameAggregate.Specifications.Queries
{
    public class CheckPlayerConnectedToGameSpecification : BaseSpecification<Game>
    {
        public CheckPlayerConnectedToGameSpecification(GameId gameId, UserId userId)
            : base(g => g.Id == gameId && g.GamePlayers.Any(p => p.UserId == userId)) { }
    }
}
