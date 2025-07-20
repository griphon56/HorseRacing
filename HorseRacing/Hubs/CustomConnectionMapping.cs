namespace HorseRacing.Api.Hubs
{
    /// <summary>
	/// Маппер хаба - SignalR
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CustomConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connections =
            new Dictionary<T, HashSet<string>>();

        /// <summary>
        /// Количество соединений
        /// </summary>
        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        /// <summary>
        /// Метод установки соединения
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="connectionId">Код соединения</param>
        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }
        /// <summary>
        /// Метод получения списка соединений
        /// </summary>
        /// <param name="key">Ключ</param>
        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public IEnumerable<T> GetAllKeys() => _connections.Keys;

        /// <summary>
        /// Метод отключения соединения
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="connectionId">Код соединения</param>
        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}
