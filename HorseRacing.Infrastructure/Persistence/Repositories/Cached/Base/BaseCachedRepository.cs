using HorseRacing.Application.Common.Interfaces.Persistence.Base;
using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Domain.Common.Caching.Configuration;
using HorseRacing.Domain.Common.Models.Base;
using HorseRacing.Infrastructure.Persistence.DbContexts;
using HorseRacing.Infrastructure.Persistence.Repositories.Base;

namespace HorseRacing.Infrastructure.Persistence.Repositories.Cached.Base
{
    /// <summary>
	/// Базовый кэшированный репозиторий.
	/// </summary>
	public class BaseCachedRepository<T, Y, R> : BaseRepository<T, Y>, IBaseRepository<T, Y>
        where T : AggregateRootGuid<Y>
        where Y : IdentityBaseGuid
        where R : IBaseRepository<T, Y>
    {
        protected readonly R _decorated;
        /// <summary>
        /// Конфигурационные настройки кеша.
        /// </summary>
        protected readonly CacheConfigurationSettings _cacheConfigurationSettings;
        /// <summary>
		/// Общий ключ для хранения списка связанных дочерних ключей (для очистки кеша)
		/// </summary>
		protected readonly Type _keyForStoreListKeyNames;
        private readonly IExtendedCacheService _cacheService;
        /// <summary>
        ///  Инициализирует новый экземпляр класса <see cref="BaseCachedRepository"/> .
        /// </summary>
        /// <param name="memoryCache">Кэш памяти.</param>
        public BaseCachedRepository(HRDbContext dbContext, CacheConfigurationSettings cacheConfigurationSettings
            , IExtendedCacheService cacheService, R decorated) : base(dbContext)
        {
            _decorated = decorated;
            _cacheConfigurationSettings = cacheConfigurationSettings;
            _cacheService = cacheService;
            _keyForStoreListKeyNames = _decorated.GetType().GetInterfaces().LastOrDefault();
        }
        protected virtual void OnChangeData(string methodName, List<T> entities)
        {
            if (entities is null || entities.Count == 0)
            {
                return;
            }
            if (nameof(IBaseRepository<T, Y>.Update) == methodName ||
                nameof(IBaseRepository<T, Y>.Remove) == methodName)
            {
                _cacheService.RemoveData(CacheRepositoryUtilities.FormatKey(FormatDefaultKeyForGetByIdMethod(entities[0].Id)));
            }
        }
        public new async Task Add(T entity)
        {
            await base.Add(entity);
            OnChangeData(nameof(BaseRepository<T, Y>.Update), new List<T>() { entity });
        }

        public new async Task AddRange(IEnumerable<T> entities)
        {
            await base.AddRange(entities);
            OnChangeData(nameof(BaseRepository<T, Y>.Update), new List<T>() { entities.FirstOrDefault() });
        }
        /// <summary>
        /// Дефолтный метод формирования ключа кэша для получения элемента по Id
        /// </summary>
        /// <param name="id">идентификатор</param>
        /// <returns></returns>
        private string FormatDefaultKeyForGetByIdMethod(Y id)
        {
            return CacheRepositoryUtilities.FormatKey($"{typeof(Y).FullName}.GetById", id.Value.ToString());
        }
        public new async Task<T?> GetById(Y id)
        {
            if (id.Value == Guid.Empty) { return null; }
            return await _cacheService.GetOrSetData<T>([id.Value.ToString()],
               async () => { return await base.GetById(id); }, _keyForStoreListKeyNames.ToString());
        }
        public new async Task Update(T entity)
        {
            await base.Update(entity);
            OnChangeData(nameof(BaseRepository<T, Y>.Update), new List<T>() { entity });
        }
        public new async Task Remove(T entity)
        {
            await base.Remove(entity);
            OnChangeData(nameof(BaseRepository<T, Y>.Remove), new List<T>() { entity });
        }
    }
}
