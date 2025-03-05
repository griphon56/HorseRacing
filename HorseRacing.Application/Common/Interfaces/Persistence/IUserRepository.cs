using HorseRacing.Application.Common.Interfaces.Persistence.Base;
using HorseRacing.Domain.UserAggregate;
using HorseRacing.Domain.UserAggregate.ReadOnlyModels;
using HorseRacing.Domain.UserAggregate.ValueObjects;

namespace HorseRacing.Application.Common.Interfaces.Persistence
{
    public interface IUserRepository : IBaseRepository<User, UserId>
    {
        /// <summary>
        /// Метод получения пользователя по логину.
        /// </summary>
        /// <param name="userName">Логин пользователя.</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<User?> GetUserByUserName(string userName, CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод получения представлений всех актуальных пользователей.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Возвращает список представлений пользователей (List<UserView>).</returns>
        Task<List<UserView>> GetAllUserViews(CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод получения списка логинов пользователей.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Возвращает список логинов пользователей (List<string>).</returns>
        Task<List<string>> GetAllUserNames(CancellationToken cancellationToken = default);
    }
}
