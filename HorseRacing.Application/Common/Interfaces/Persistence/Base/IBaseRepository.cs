using HorseRacing.Domain.Common.Models.Base;
using System.Linq.Expressions;

namespace HorseRacing.Application.Common.Interfaces.Persistence.Base
{
    public interface IBaseRepository<T, Y>
        where T : AggregateRootGuid<Y>
        where Y : IdentityBaseGuid
    {
        /// <summary>
        /// Отсоединение сущности от контекста (если возникает ошибка связанная с выбором одной сущности несколько раз в одном блоке кода - решение: использовать данный метод для неиспользуемых сущностей)
        /// </summary>
        /// <param name="t"></param>
        void Detach(T t);
        /// <summary>
        /// Метод добавления записи в БД
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task Add(T entity, CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод добавления множества записей в БД
        /// </summary>
        /// <param name="entities">Список сущностей</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task AddRange(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод получения записи по ID
        /// </summary>
        /// <param name="id">Код записи</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <param name="needAsNoTracking">Признак использования "AsNoTracking"</param>
        Task<T?> GetById(Y id, CancellationToken cancellationToken = default, bool needAsNoTracking = false);
        /// <summary>
        /// Метод получения списка записей по списку кодов сущности
        /// </summary>
        /// <param name="ids">Список кодов</param>
        /// <param name="order">Сортировка</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<List<T>> GetEntities(List<Y> ids, Func<IQueryable<T>, IOrderedQueryable<T>>? order = null
            , CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод получения списка записей шаблонной модели представления
        /// </summary>
        /// <typeparam name="TResultView">Результирующая модель представления </typeparam>
        /// <param name="ids">Список кодов</param>
        /// <param name="select">Выборка</param>
        /// <param name="order">Сортировка</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<List<TResultView>> GetEntities<TResultView>(List<Y> ids, Expression<Func<T, TResultView>> select
            , Func<IQueryable<T>, IOrderedQueryable<T>>? order = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод обновления записи в БД
        /// </summary>
        /// <param name="entity">Сущность</param>
        Task Update(T entity, CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод обновления списка записей в БД
        /// </summary>
        /// <param name="entities">Список сущностей</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task UpdateRange(List<T> entities, CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод удаления записи из БД
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task Remove(T entity, CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод удаления множества записей в БД
        /// </summary>
        /// <param name="entities">Список сущностей</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task RemoveRange(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}
