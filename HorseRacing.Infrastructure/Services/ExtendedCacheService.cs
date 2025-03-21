using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Domain.Common.Caching.Configuration;
using HorseRacing.Infrastructure.Persistence.Repositories.Cached.Base;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using static HorseRacing.Application.Common.Interfaces.Services.IExtendedCacheService;

namespace HorseRacing.Infrastructure.Services
{
    /// <summary>
	/// Сервис кеширования
	/// </summary>
	public class ExtendedCacheService : CacheService, IExtendedCacheService
    {
        private readonly SemaphoreSlim _cacheLock;
        public ExtendedCacheService(IRedisCacheService redisCacheService,
            IOptions<CacheConfigurationSettings> cacheConfigurationSettings) : base(redisCacheService,  cacheConfigurationSettings)
        {
            _cacheLock = new SemaphoreSlim(1);
        }

        /// <summary>
        /// Метод получения закешированных данных и сохранения данных в кеш при их отсутствии там.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyEntries">Набор значений для создания уникального ключа (формируется совместно с commonKey) </param>
        /// <param name="setDataAction">Делегат для получения данных</param>
        /// <param name="keyForStoreListKeyNames">Общий ключ для хранения списка связанных дочерних ключей (для очистки кеша)</param>
        /// <param name="commonKey">Ключ записи - берется название метода, из которого вызывается данный, иначе - можно задать вручную</param>
        /// <returns></returns>
        public async Task<T?> GetOrSetData<T>(string[] keyEntries, setData<T> setDataAction,
            string keyForStoreListKeyNames = "", [CallerMemberName] string commonKey = "")
        {
            if (string.IsNullOrEmpty(commonKey))
            {
                throw new Exception("Ключ для записи в кэш не может иметь пустое значение");
            }
            return await GetOrSetDataBase(CacheRepositoryUtilities.FormatKey(keyEntries, commonKey),
                setDataAction, keyForStoreListKeyNames);
        }
        /// <summary>
        /// Метод получения закешированных данных и сохранения данных в кеш при их отсутствии там.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="setDataAction">Делегат для получения данных</param>
        /// <param name="keyForStoreListKeyNames">Общий ключ для хранения списка связанных дочерних ключей (для очистки кеша)</param>
        /// <returns></returns>
        private async Task<T?> GetOrSetDataBase<T>(string key, setData<T> setDataAction, string keyForStoreListKeyNames = "")
        {
            var cachedData = GetCachedData<T>(key);
            if (cachedData is not null)
            {
                return cachedData;
            }
            // блокируем поток, если какой-то поток уже вошел в этот блок
            await _cacheLock.WaitAsync();
            try
            {
                // еще раз проверяем - может быть какой-то поток уже записал в этот ключ значение
                cachedData = GetCachedData<T>(key);
                if (cachedData is not null)
                {
                    return cachedData;
                }
                // если не записал, тогда записываем
                cachedData = await setDataAction();
                if (cachedData is not null)
                {
                    SetData(cachedData, key);
                    if (!string.IsNullOrEmpty(keyForStoreListKeyNames))
                    {
                        var existingKeys = GetCachedData<List<string>>(keyForStoreListKeyNames);
                        if (existingKeys != null)
                        {
                            existingKeys.Add(key);
                        }
                        else
                        {
                            existingKeys = new List<string> { key };
                        }
                        SetData(existingKeys, keyForStoreListKeyNames);
                    }

                    return cachedData;
                }
                else return default;
            }
            finally
            {
                //снимаем блокировку
                _cacheLock.Release();
            }
        }

        /// <summary>
        /// Очистка списка значений в кеше по ключам, хранящимся списком в кеше по общему ключу
        /// </summary>
        /// <param name="commonKey">Общий ключ для хранения списка дочерних ключей</param>
        /// <returns></returns>
        public object RemoveDataByKeysList(string commonKey)
        {
            var cacheKeysList = GetCachedData<List<string>>(commonKey);
            if (cacheKeysList != null)
            {
                foreach (var key in cacheKeysList)
                    RemoveData(key);
                RemoveData(commonKey);
            }
            return true;
        }
        /// <summary>
        /// Очистка списка значений в кеше по ключам, хранящимся списком в кеше по общему ключу
        /// </summary>
        /// <param name="commonKeys">Список наименований общих ключей, используемых для хранения списка наименований дочерних ключей</param>
        /// <param name="keyParts">Список частей наименований дочерних ключей, по которым будет происходит поиск и удаление данных в кеше</param>
        /// <returns></returns>
        public object RemoveDataByKeyPartsList(string[] commonKeys, string[] keyParts)
        {
            if (commonKeys == null || commonKeys.Length == 0 || keyParts == null || keyParts.Length == 0) return true;

            foreach (var commonKey in commonKeys)
            {
                var cacheKeysList = GetCachedData<List<string>>(commonKey);
                if (cacheKeysList != null)
                {
                    foreach (var key in cacheKeysList)
                    {
                        foreach (var keyPart in keyParts)
                        {
                            if (!string.IsNullOrEmpty(keyPart) && key.ToLower().Contains(keyPart.ToLower()))
                            {
                                RemoveData(key);
                            }
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Очистка списка значений в кеше по ключам, хранящимся списком в кеше по общему ключу
        /// </summary>
        /// <param name="commonKeys">Список типов, выступающих в качестве наименований общих ключей, используемых для хранения списка наименований дочерних ключей</param>
        /// <param name="keyParts">Список частей наименований дочерних ключей, по которым будет происходит поиск и удаление данных в кеше</param>
        /// <returns></returns>
        public object RemoveDataByKeyPartsList(Type[] commonKeys, string[] keyParts)
        {
            if (commonKeys == null || commonKeys.Length == 0 || keyParts == null || keyParts.Length == 0) return true;

            foreach (Type commonKey in commonKeys)
            {
                var cacheKeysList = GetCachedData<List<string>>(commonKey.FullName);
                if (cacheKeysList != null)
                {
                    foreach (var key in cacheKeysList)
                    {
                        foreach (var keyPart in keyParts)
                        {
                            if (!string.IsNullOrEmpty(keyPart) && key.ToLower().Contains(keyPart.ToLower()))
                            {
                                RemoveData(key);
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
