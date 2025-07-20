namespace HorseRacing.Application.Common.Interfaces.Services
{
    public interface IOuterCommonHubCallService
    {
        /// <summary>
        /// Отправка уведомления что все пользователи присоединились к игре
        /// </summary>
        /// <param name="gameId">Код игры</param>
        Task AllPlayersJoinToGame(Guid gameId);
    }
}
