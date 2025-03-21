using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Domain.Common.Caching.Configuration;
using JsonNet.ContractResolvers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace HorseRacing.Infrastructure.Services.RedisCache
{
    /// <summary>
    /// Сервис кеширования с использованием Redis
    /// </summary>
    public class RedisCacheService : IRedisCacheService
    {
        protected readonly IOptions<CacheConfigurationSettings> _cacheConfigurationSettings;
        private IDatabase? _db;
        private IServer? _server;
        public RedisCacheService(IOptions<CacheConfigurationSettings> cacheConfigurationSettings)
        {
            _cacheConfigurationSettings = cacheConfigurationSettings;
            if (cacheConfigurationSettings.Value.CacheVariant == CacheConfigurationSettings.RedisCache &&
                !string.IsNullOrEmpty(cacheConfigurationSettings.Value.RedisURL))
            {
                ConfigureRedis(cacheConfigurationSettings.Value.RedisURL);
            }
        }
        private void ConfigureRedis(string redisUrl)
        {
            var connHelper = new ConnectionHelper(redisUrl);

            _server = connHelper.Connection.GetServer(redisUrl);
            _db = connHelper.Connection.GetDatabase();
        }
        
        public T? GetCachedData<T>(string key)
        {
            if (_db == null) return default;
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value!, new JsonSerializerSettings
                {
                    ContractResolver = new PrivateSetterContractResolver()
                })!;
            }
            else return default;
        }

        public bool SetData<T>(T value, string key)
        {
            if (_db == null) return false;
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), new TimeSpan(0, _cacheConfigurationSettings.Value.TimeoutMinutes, 0));
            return isSet;
        }
        
        public object RemoveData(string key)
        {
            if (_db == null) return false;
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }

        public async Task ClearAllCache()
        {
            if (_db != null && _server != null)
            {
                await _server.FlushDatabaseAsync(_db.Database);
            }
        }

        public class ConnectionHelper
        {
            public ConnectionHelper(string redisUrl)
            {
                lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                {
                    var options = ConfigurationOptions.Parse(redisUrl);
                    options.AllowAdmin = true;
                    return ConnectionMultiplexer.Connect(options);
                });
                Connection = lazyConnection.Value;
            }
            private static Lazy<ConnectionMultiplexer>? lazyConnection;
            public ConnectionMultiplexer Connection
            { get; private set; }
        }
    }
}
