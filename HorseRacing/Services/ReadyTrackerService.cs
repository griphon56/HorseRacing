using System.Collections.Concurrent;

namespace HorseRacing.Api.Services
{
    public class ReadyTrackerService
    {
        private readonly ConcurrentDictionary<Guid, HashSet<Guid>> _gameReadyUsers = new();

        public void MarkReady(Guid gameId, Guid userId)
        {
            _gameReadyUsers.AddOrUpdate(
                gameId,
                _ => new HashSet<Guid> { userId },
                (_, existingSet) =>
                {
                    lock (existingSet)
                    {
                        existingSet.Add(userId);
                    }
                    return existingSet;
                });
        }

        public int CountReady(Guid gameId)
        {
            if (_gameReadyUsers.TryGetValue(gameId, out var users))
            {
                lock (users)
                {
                    return users.Count;
                }
            }
            return 0;
        }

        public void Clear(Guid gameId)
        {
            _gameReadyUsers.TryRemove(gameId, out _);
        }
    }
}
