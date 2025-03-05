using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Infrastructure.Persistence.DbContexts;
using HorseRacing.Infrastructure.Persistence.Repositories.Base;

namespace HorseRacing.Infrastructure.Persistence.Repositories.Common
{
    public class GameRepository : BaseRepository<Game, GameId>, IGameRepository
    {
        private readonly HRDbContext _dbContext;

        public GameRepository(HRDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


    }
}
