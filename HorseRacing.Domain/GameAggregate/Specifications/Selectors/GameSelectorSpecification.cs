using HorseRacing.Domain.GameAggregate.ReadOnlyModels;
using System.Linq.Expressions;

namespace HorseRacing.Domain.GameAggregate.Specifications.Selectors
{
    public static class GameSelectorSpecification
    {
        /// <summary>
        /// Спецификация выборки модели игры
        /// </summary>
        public static Expression<Func<Game, GameView>> GameViewSelectorSpecification()
        {
            return m => new GameView()
            {
                GameId = m.Id,
                Name = m.Name,
                Mode = m.Mode,
                Status = m.Status,
                DateStart = m.DateStart,
                DateEnd = m.DateEnd
            };
        }
    }
}
