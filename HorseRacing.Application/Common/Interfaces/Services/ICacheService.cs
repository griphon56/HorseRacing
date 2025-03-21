namespace HorseRacing.Application.Common.Interfaces.Services
{
    /// <summary>
	/// Интерфейс сервиса кеширования
	/// </summary>
	public interface ICacheService
    {
        /// <summary>
        /// Метод получения закешированных данных
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Ключ</param>
        public T? GetCachedData<T>(string key);
        /// <summary>
        /// Сохранение данных в кеш
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="key">Ключ</param>
        public bool SetData<T>(T value, string key);
        /// <summary>
        /// Очистка данных в кеше по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        public object RemoveData(string key);
        /// <summary>
        /// Очищает весь кэш
        /// </summary>
        public Task ClearAllCache();
    }
}
