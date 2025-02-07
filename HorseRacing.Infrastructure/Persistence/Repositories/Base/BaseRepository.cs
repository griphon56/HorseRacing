using HorseRacing.Application.Common.Interfaces.Persistence.Base;
using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static HorseRacing.Common.CommonSystemValues;

namespace HorseRacing.Infrastructure.Persistence.Repositories.Base
{
    public class BaseRepository<T, Y> : IBaseRepository<T, Y>
        where T : AggregateRootGuid<Y>
        where Y : IdentityBaseGuid
    {
        private readonly HRDbContext _dbContext;

        public BaseRepository(HRDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Спецификация поиска ключу
        /// </summary>
        /// <param name="id">Ключ записи</param>
        /// <returns></returns>
        protected Expression<Func<T, bool>> ByKeySearchSpecification(Y id)
        {
            return x => x.Id == id;
        }
        /// <summary>
        /// Спецификация поиска ключам
        /// </summary>
        /// <param name="ids">Ключи записей</param>
        /// <returns></returns>
        protected Expression<Func<T, bool>> ByKeysSearchSpecification(IEnumerable<Y> ids)
        {
            return x => ids.Contains(x.Id);
        }
        /// <summary>
        /// Метод сборки запроса
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="order">Сортировка</param>
        /// <param name="skip">Количество записей, которое необходимо пропустить</param>
        /// <param name="take">Количество записей, которое необходимо пропустить</param>
        /// <param name="needAsNoTracking">Признак использования "AsNoTracking"</param>
        protected IQueryable<T> BuildQuery(Expression<Func<T, bool>>? filter = null
            , Func<IQueryable<T>, IOrderedQueryable<T>>? order = null, int? skip = null, int? take = null
            , bool needAsNoTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (needAsNoTracking)
            {
                query = query.AsNoTracking();
            }

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (order is not null)
            {
                query = order(query);
            }

            if (skip is not null && skip >= 0)
            {
                query = query.Skip(skip.Value);
            }

            if (take is not null && take > 0)
            {
                query = query.Take(take.Value);
            }

            return query;
        }
        /// <summary>
        /// Метод получения записей по частям
        /// </summary>
        /// <typeparam name="TResultView"></typeparam>
        /// <param name="filter">Фильтр</param>
        /// <param name="select">Выборка</param>
        /// <param name="order">Сортировка</param>
        /// <param name="batchSize">Максимальное количество записей для получения из запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        private async Task<List<TResultView>> FetchInBatches<TResultView>(Expression<Func<T, bool>> filter
            , Expression<Func<T, TResultView>> select, Func<IQueryable<T>, IOrderedQueryable<T>>? order = null
            , int batchSize = CommonRepositoryValues.MaxTakeInContainsQuery, CancellationToken cancellationToken = default)
        {
            var result = new List<TResultView>();
            int skip = 0;

            while (true)
            {
                var batch = await BuildQuery(filter, order, skip, batchSize)
                    .Select(select)
                    .ToListAsync(cancellationToken);

                if (!batch.Any())
                {
                    return result;
                }

                result.AddRange(batch);
                skip += batchSize;
            }
        }

        public void Detach(T t)
        {
            _dbContext.Set<T>().Entry(t).State = EntityState.Detached;
        }

        public async Task<T?> GetById(Y id, CancellationToken cancellationToken = default, bool needAsNoTracking = true)
        {
            var filter = ByKeySearchSpecification(id);
            return await BuildQuery(filter, needAsNoTracking: needAsNoTracking).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<TResultView>> GetEntities<TResultView>(List<Y> ids, Expression<Func<T, TResultView>> select
            , Func<IQueryable<T>, IOrderedQueryable<T>>? order = null, CancellationToken cancellationToken = default)
        {
            var entities = new List<TResultView>();

            if (ids is null || !ids.Any())
            {
                return entities;
            }

            var filter = ByKeysSearchSpecification(ids);

            if (ids.Count >= CommonRepositoryValues.ContainsThresholdValue)
            {
                entities = await FetchInBatches(filter, select, order, cancellationToken: cancellationToken);
                return entities;
            }

            entities = await BuildQuery(filter, order).Select(select).ToListAsync(cancellationToken);
            return entities;
        }

        public async Task<List<T>> GetEntities(List<Y> ids, Func<IQueryable<T>, IOrderedQueryable<T>>? order = null
            , CancellationToken cancellationToken = default)
        {
            return await GetEntities(ids, m => m, order, cancellationToken);
        }

        public async Task Add(T entity, CancellationToken cancellationToken = default)
        {
            if (entity is not null)
            {
                await _dbContext.Set<T>().AddAsync(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task AddRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities is not null && entities.Count() > 0)
            {
                await _dbContext.Set<T>().AddRangeAsync(entities);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task Update(T entity, CancellationToken cancellationToken = default)
        {
            if (entity is not null)
            {
                _dbContext.Set<T>().Update(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateRange(List<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities is not null && entities.Count > 0)
            {
                _dbContext.Set<T>().UpdateRange(entities);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task Remove(T entity, CancellationToken cancellationToken = default)
        {
            if (entity is not null)
            {
                _dbContext.Set<T>().Remove(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task RemoveRange(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            if (entities is not null && entities.Count() > 0)
            {
                _dbContext.Set<T>().RemoveRange(entities);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
