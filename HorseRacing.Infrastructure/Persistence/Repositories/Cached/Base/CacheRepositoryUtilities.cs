using System.Runtime.CompilerServices;

namespace HorseRacing.Infrastructure.Persistence.Repositories.Cached.Base
{
    public static class CacheRepositoryUtilities
    {
        /// <summary>
        /// Создаем ключ для записи кэша
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="values">Список значений</param>
        /// <returns>Возвращает значение строки.</returns>
        public static string FormatKey(string key, params string[] values)
        {
            if (values is null || values.Length == 0) return $"{key}";
            var fullKey = $"{key}";
            foreach (var value in values)
            {
                fullKey += $"_{value}";
            }
            return fullKey;
        }

        public static string FormatKey(string[] values, [CallerMemberName] string key = "")
        {
            return FormatKey(key, values);
        }
    }
}
