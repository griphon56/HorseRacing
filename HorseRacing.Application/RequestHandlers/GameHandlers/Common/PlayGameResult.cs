using HorseRacing.Application.Base;
using HorseRacing.Domain.GameAggregate.Entities;
using HorseRacing.Domain.GameAggregate.ValueObjects;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Common
{
    public class PlayGameResult : BaseModelResult
    {
        /// <summary>
        /// Код игры
        /// </summary>
        public GameId GameId { get; set; }
        /// <summary>
        /// Колода до начала игры
        /// </summary>
        public List<GameDeckCard> InitialDeck { get; set; } // передалать на view

        GamePlayer + GameHorse

        /// <summary>
        /// Все события по шагам
        /// </summary>
        public List<GameEvent> Events { get; set; }
    }
}
