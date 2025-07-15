using HorseRacing.Application.Base;
using HorseRacing.Domain.GameAggregate.ReadOnlyModels;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Common
{
    public class GetLobbyUsersWithBetsResult : BaseModelResult
    {
        public LobbyUsersWithBetsView Data { get; set; } = new();
    }
}
