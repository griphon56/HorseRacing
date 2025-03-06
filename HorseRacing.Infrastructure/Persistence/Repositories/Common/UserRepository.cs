using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Domain.UserAggregate;
using HorseRacing.Domain.UserAggregate.ReadOnlyModels;
using HorseRacing.Domain.UserAggregate.Specifications;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using HorseRacing.Infrastructure.Persistence.DbContexts;
using HorseRacing.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace HorseRacing.Infrastructure.Persistence.Repositories.Common
{
    public class UserRepository : BaseRepository<User, UserId>, IUserRepository
    {
        private readonly HRDbContext _dbContext;

        public UserRepository(HRDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByUserName(string userName, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AsNoTracking()
                .Where(new GetUserByUserNameSpecification(userName)).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<UserView>> GetAllUserViews(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AsNoTracking().Where(new GetActualUserSpecification())
                .Select(user => new UserView()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName ?? "",
                    Email = user.Email ?? "",
                    Phone = user.Phone ?? "",
                    IsRemoved = user.IsRemoved
                }).ToListAsync(cancellationToken);
        }

        public async Task<List<string>> GetAllUserNames(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.AsNoTracking().Where(new GetActualUserSpecification())
                .Select(user => user.UserName).ToListAsync(cancellationToken);
        }

        public async Task<UserView?> GetUserViewByUserId(UserId id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.Where(ByKeySearchSpecification(id))
                .Select(user => new UserView()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email ?? "",
                    Phone = user.Phone ?? "",
                    IsRemoved = user.IsRemoved
                }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
