using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Common;
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

        public async Task<List<GameResultView>> GetGameResults(GameId id, CancellationToken cancellationToken = default)
        {
            return await BuildQuery(ByKeySearchSpecification(id))
                .SelectMany(game => game.GameResults
                    .Join(_dbContext.Users, result => result.UserId, user => user.Id
                        , (result, user) => new { result, user }))
                .Select(x => new GameResultView()
                {
                    GameId = id,
                    UserId = x.result.UserId,
                    BetSuit = x.result.BetSuit,
                    FullName = $"{x.user.FirstName} {x.user.LastName} ({x.user.UserName})",
                    Position = x.result.Position,
                    IsWinner = x.result.Position == CommonSystemValues.NumberOfObstacles + 1
                })
                .ToListAsync(cancellationToken);
        }
    }
}
