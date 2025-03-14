using HorseRacing.Application.Base;
using HorseRacing.Domain.GameAggregate.ReadOnlyModels;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Common
{
    public class GetAvailableSuitResult: BaseResult
    {
        public required List<GameAvailableSuitView> AvailableSuits { get; set; }
    }
}
