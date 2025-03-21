using System.Runtime.CompilerServices;

namespace HorseRacing.Application.Common.Interfaces.Services
{
    public interface IExtendedCacheService : ICacheService
    {
        public delegate Task<T?> setData<T>();
        /// <summary>
        /// Метод получения закешированных данных и сохранения данных в кеш при их отсутствии там.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyEntries">Набор значений для создания уникального ключа (формируется совместно с commonKey) </param>
        /// <param name="setDataAction">Делегат для получения данных</param>
        /// <param name="keyForStoreListKeyNames">Общий ключ для хранения списка связанных дочерних ключей (для очистки кеша)</param>
        /// <param name="commonKey">Ключ записи - берется название метода, из которого вызывается данный, иначе - можно задать вручную</param>
        /// <returns></returns>
        Task<T?> GetOrSetData<T>(string[] keyEntries, setData<T> setDataAction, string keyForStoreListKeyNames, [CallerMemberName] string commonKey = "");
        /// <summary>
        /// Очистка списка значений в кеше по ключам, хранящимся списком в кеше по общему ключу
        /// </summary>
        /// <param name="commonKeys">Список наименований общих ключей, используемых для хранения списка наименований дочерних ключей</param>
        /// <param name="keyParts">Список частей наименований дочерних ключей, по которым будет происходит поиск и удаление данных в кеше</param>
        /// <returns></returns>
        public object RemoveDataByKeysList(string commonKey);
        /// <summary>
        /// Очистка списка значений в кеше по ключам, хранящимся списком в кеше по общему ключу
        /// </summary>
        /// <param name="commonKeys">Список типов, выступающих в качестве наименований общих ключей, используемых для хранения списка наименований дочерних ключей</param>
        /// <param name="keyParts">Список частей наименований дочерних ключей, по которым будет происходит поиск и удаление данных в кеше</param>
        /// <returns></returns>
        public object RemoveDataByKeyPartsList(Type[] commonKeys, string[] keysParts);
    }
}
