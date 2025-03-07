using HorseRacing.Application.Base;
using HorseRacing.Domain.GameAggregate.ReadOnlyModels;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Common
{
    public class GetWaitingGamesResult : BaseModelResult
    {
        public List<GameView> Games { get; set; } = new List<GameView>();
    }
}
