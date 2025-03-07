using HorseRacing.Application.Base;
using HorseRacing.Domain.GameAggregate.ReadOnlyModels;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Common
{
    public class GetGameResultsResult : BaseModelResult
    {
        public List<GameResultView> GameResults { get; set; } = new List<GameResultView>();
    }
}
