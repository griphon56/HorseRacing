namespace HorseRacing.Application.Common.Interfaces.Hubs
{
    public interface ICommonServerHub
    {
        Task JoinToGame(string gameId);

        Task StartGame();
    }
}
