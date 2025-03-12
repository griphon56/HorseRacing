using HorseRacing.Application.Common.Interfaces.Persistence.Base;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.ReadOnlyModels;
using HorseRacing.Domain.GameAggregate.ValueObjects;

namespace HorseRacing.Application.Common.Interfaces.Persistence
{
    public interface IGameRepository : IBaseRepository<Game, GameId>
    {
        /// <summary>
        /// Метод получения списка игр в ожидании
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<List<GameView>> GetWaitingGames(CancellationToken cancellationToken = default);
    }
}
