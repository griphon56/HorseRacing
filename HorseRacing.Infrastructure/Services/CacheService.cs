using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Domain.Common.Caching.Configuration;
using Microsoft.Extensions.Options;

namespace HorseRacing.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        protected readonly IOptions<CacheConfigurationSettings> _cacheConfigurationSettings;
        protected readonly IRedisCacheService _redisCacheService;

        public CacheService(IRedisCacheService redisCacheService, IOptions<CacheConfigurationSettings> cacheConfigurationSettings)
        {
            _cacheConfigurationSettings = cacheConfigurationSettings;
            _redisCacheService = redisCacheService;
        }
        /// <summary>
        /// Метод получения закешированных данных
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Ключ</param>
        public T? GetCachedData<T>(string key)
        {
            return _cacheConfigurationSettings.Value.CacheVariant switch
            {
                CacheConfigurationSettings.RedisCache => _redisCacheService.GetCachedData<T>(key),
                _ => default
            };
        }

        /// <summary>
        /// Сохранение данных в кеш
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="key">Ключ</param>
        public bool SetData<T>(T value, string key)
        {
            return _cacheConfigurationSettings.Value.CacheVariant switch
            {
                CacheConfigurationSettings.RedisCache => _redisCacheService.SetData(value, key),
                _ => default

            };
        }
        /// <summary>
        /// Очистка данных в кеше по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        public object RemoveData(string key)
        {
            return _cacheConfigurationSettings.Value.CacheVariant switch
            {
                CacheConfigurationSettings.RedisCache => _redisCacheService.RemoveData(key),
                _ => true
            };
        }
        /// <summary>
        /// Очищает весь кэш
        /// </summary>
        public async Task ClearAllCache()
        {
            switch (_cacheConfigurationSettings.Value.CacheVariant)
            {
                case CacheConfigurationSettings.RedisCache:
                    await _redisCacheService.ClearAllCache();
                    break;
                default:
                    break;
            }
        }
    }
}
