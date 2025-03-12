using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.ReadOnlyModels;
using HorseRacing.Domain.GameAggregate.Specifications.Queries;
using HorseRacing.Domain.GameAggregate.Specifications.Selectors;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Infrastructure.Persistence.DbContexts;
using HorseRacing.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace HorseRacing.Infrastructure.Persistence.Repositories.Common
{
    public class GameRepository : BaseRepository<Game, GameId>, IGameRepository
    {
        private readonly HRDbContext _dbContext;

        public GameRepository(HRDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<GameView>> GetWaitingGames(CancellationToken cancellationToken = default)
        {
            return await BuildQuery(new GetGameByStatusWaitSpecification())
                .Select(GameSelectorSpecification.GameViewSelectorSpecification()).ToListAsync(cancellationToken);
        }
    }
}
