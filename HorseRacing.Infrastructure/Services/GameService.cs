using ErrorOr;
using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Domain.GameAggregate;
using HorseRacing.Domain.GameAggregate.Enums;

namespace HorseRacing.Infrastructure.Services
{
    public class GameService : IGameService
    {
        public Task<ErrorOr<SuitType>> GetRandomAvilableSuit(Game game)
        {
            List<SuitType> exclude = new List<SuitType>() { SuitType.None }; 
            exclude.AddRange(game.GamePlayers.Select(x => x.BetSuit).ToList());

            Random rnd = new Random();

            List<SuitType> values = Enum.GetValues(typeof(SuitType)).Cast<SuitType>().ToList();
            var availableSuits = values.Except(exclude).ToList();

            var result = availableSuits[rnd.Next(availableSuits.Count)];
            return Task.FromResult<ErrorOr<SuitType>>(result);
        }
    }
}
