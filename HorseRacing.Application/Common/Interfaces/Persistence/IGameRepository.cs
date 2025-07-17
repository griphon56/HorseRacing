using HorseRacing.Application.Common.Interfaces.Persistence.Base;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.ReadOnlyModels;
using HorseRacing.Domain.GameAggregate.ValueObjects;
using HorseRacing.Domain.UserAggregate.ValueObjects;

namespace HorseRacing.Application.Common.Interfaces.Persistence
{
    public interface IGameRepository : IBaseRepository<Game, GameId>
    {
        /// <summary>
        /// Метод получения списка игр в ожидании
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<List<GameView>> GetWaitingGames(CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод получения результатов игры
        /// </summary>
        /// <param name="id">Код игры</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<List<GameResultView>> GetGameResults(GameId id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод получения пользоватлей в лобби с их ставками
        /// </summary>
        /// <param name="id">Код игры</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<LobbyUsersWithBetsView> GetLobbyUsersWithBets(GameId id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Метод проверки подключения пользователя к игре
        /// </summary>
        /// <param name="gameId">Код игры</param>
        /// <param name="userId">Код пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<bool> CheckPlayerConnectedToGame(GameId gameId, UserId userId, CancellationToken cancellationToken = default);
    }
}
