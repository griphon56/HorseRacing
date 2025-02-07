namespace HorseRacing.Domain.Common.Caching.Configuration
{
    /// <summary>
	/// Настройки кэширования
	/// </summary>
	public class CacheConfigurationSettings
    {
        /// <summary>
        /// Название раздела.
        /// </summary>
        public const string SectionName = $"{nameof(CacheConfigurationSettings)}";
        /// <summary>
        /// Перерыв в минутах для кэш
        /// </summary>
        public int TimeoutMinutes { get; init; }
        /// <summary>
        /// Кэширование с помощью MemoryCache
        /// </summary>
        public const string MemoryCache = nameof(MemoryCache);
        /// <summary>
        /// Кэширование с помощью Redis
        /// </summary>
        public const string RedisCache = nameof(RedisCache);
        /// <summary>
        /// Вариант кеширования.
        /// </summary>
        /// <value>Строка.</value>
        public string CacheVariant { get; init; } = string.Empty;
        public string RedisURL { get; init; } = string.Empty;
    }
}
