using ErrorOr;
using HorseRacing.Domain.UserAggregate.ReadOnlyModels;

namespace HorseRacing.Application.Common.Interfaces.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Получение данных о Пользователе по логину
        /// </summary>
        /// <param name="userName">Логин</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<ErrorOr<UserView?>> GetWorkingUser(string? userName = null, CancellationToken cancellationToken = default);
    }
}
