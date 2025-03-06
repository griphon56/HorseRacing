using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Persistence;
using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Domain.UserAggregate;
using HorseRacing.Domain.UserAggregate.ReadOnlyModels;
using HorseRacing.Domain.Common.Errors;

namespace HorseRacing.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextUserService _httpContextUserService;
        private readonly IUserRepository _userRepository;
        public UserService(IHttpContextUserService httpContextUserService, IUserRepository userRepository)
        {
            _httpContextUserService = httpContextUserService;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<UserView?>> GetWorkingUser(string? userName = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userName))
            {
                var getCurrentUserName = _httpContextUserService.GetCurrentUserName();
                if (getCurrentUserName.IsError)
                {
                    return getCurrentUserName.FirstError;
                }
                userName = getCurrentUserName.Value;

            }
            return await GetUserByUserName(userName ?? "", cancellationToken);
        }
        /// <summary>
        /// Базовый метод получения пользователя по логину
        /// </summary>
        /// <param name="userName">Логин</param>
        /// <param name="cancellationToken">Токен отмены</param>
        private async Task<ErrorOr<UserView?>> GetUserByUserName(string userName, CancellationToken cancellationToken = default)
        {
            if (await _userRepository.GetUserByUserName(userName, cancellationToken) is not User user)
            {
                return Errors.Authentication.NotFoundUser;
            }
            return await _userRepository.GetUserViewByUserId(user.Id, cancellationToken);
        }
    }
}
