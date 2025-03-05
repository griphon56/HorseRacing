using HorseRacing.Application.Common.Interfaces.Persistence.Base;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.ValueObjects;

namespace HorseRacing.Application.Common.Interfaces.Persistence
{
    public interface IGameRepository : IBaseRepository<Game, GameId>
    {
    }
}
